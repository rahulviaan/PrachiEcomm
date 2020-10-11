using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.Entities;
using GleamTech.DocumentUltimateExamples.AspNetCoreCS.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadEdgeCore.Models.Interfaces;

namespace ReadEdgeCore.Controllers
{
   
    public class AccountController : Controller
    {
        private IHttpContextAccessor _HttpContextAccessor;
        private IUser _User { get; set; }
        private IPrachiuser _prachiUser { get; set; }
        private ReadEdgeUserLoginInfo _readEdgeUserLoginInfo;
        public AccountController(IUser user, IPrachiuser prachiUser, IHttpContextAccessor httpContextAccessor , ReadEdgeUserLoginInfo readEdgeUserLoginInfo)
        {
            _User = user;
            _prachiUser = prachiUser;
            _HttpContextAccessor = httpContextAccessor;
            _readEdgeUserLoginInfo = readEdgeUserLoginInfo;
        }

        public IActionResult Login()
        {
            var userid = _HttpContextAccessor.HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userid))
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string Password)
        {
            if (UserName == null || Password == null)
            {
                ViewBag.Message = "Please Enter Username and Password !!!";
                return View();

            }
                var result = _prachiUser.GetUserLogin(UserName, Password);
            //Task<DAL.Models.Entities.AspNetUsers> user = _prachiUser.GetUserLogin(UserName, Password);
            //  var result = await user;

            if (result.Result != null)
            {
                var role = _prachiUser.GetUserRole(result.Result.Id);
                var readEdgeLogin = _prachiUser.GetReadEdgeLoginByIds(result.Result.Id);
                if (readEdgeLogin != null)
                {
                   
                    if (readEdgeLogin.LoginAllowed)
                    {
                       
                        if (role == null && role.Name!="Teacher" && role != null && role.Name!= "Admin" && role.Name!= "Marketing" && role.Name!= "user" && role.Name != "Sales" && role.Name!= "School")
                        {
                            ViewBag.Message = "You are not subscribed user !!!";
                            return View();
                        }
                        var userbookidsresult = _prachiUser.GetUserBookIds(result.Result.Id);
                        string userbookids = string.Empty;
                        if (userbookidsresult.Count != 0)
                        {
                            userbookids = userbookidsresult.Select(x => Convert.ToString(x.BookId)).Aggregate((x, y) => x + ',' + y);
                        }

                        #region Sessions
                        _HttpContextAccessor.HttpContext.Session.Remove("Userbookids");
                        _HttpContextAccessor.HttpContext.Session.SetString("Userbookids", userbookids);

                        _HttpContextAccessor.HttpContext.Session.Remove("Role");
                        _HttpContextAccessor.HttpContext.Session.SetString("Role", role.Name);

                        //var roleid=_prachiUser.GetUserRoleId(result.Result.Id);
                        _HttpContextAccessor.HttpContext.Session.Remove("UserId");
                        _HttpContextAccessor.HttpContext.Session.SetString("UserId", result.Result.Id);

                        #endregion

                        readEdgeLogin.CurrentLogins = readEdgeLogin.CurrentLogins + 1;

                        if (readEdgeLogin.CurrentLogins == readEdgeLogin.AllowedSystems)
                        {
                            readEdgeLogin.LoginAllowed = false;
                        }

                        _prachiUser.UpdateReadEdgeLogin(readEdgeLogin);
                         
                        _readEdgeUserLoginInfo.Userid = result.Result.Id;
                        _readEdgeUserLoginInfo.LoginTime = DateTime.UtcNow.ToLocalTime();
                        _prachiUser.InsertReadEdgeUserLoginInfo(_readEdgeUserLoginInfo);
                        _HttpContextAccessor.HttpContext.Session.SetString("Id", _readEdgeUserLoginInfo.Id.ToString());
                        return RedirectToAction("Dashboard", "Home");

                    }
                    else {
                        ViewBag.Message = "Login limit exceeded !!!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid login attempt !!!";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Invalid login attempt !!!";
                return View();
            }



        }
    
        
        public IActionResult logoff()
        {
            var userid = _HttpContextAccessor.HttpContext.Session.GetString("UserId");
            var id = _HttpContextAccessor.HttpContext.Session.GetString("Id");
            var readEdgeLogin = _prachiUser.GetReadEdgeLoginByIds(userid);
            var readEdgeUserLoginInfo = _prachiUser.GetReadEdgeUserLoginInfoByIds(Convert.ToInt32(id));

            
          
            //if (readEdgeLogin.CurrentLogins != readEdgeLogin.AllowedSystems)
            //{
            //    readEdgeLogin.LoginAllowed = true;
            //    readEdgeLogin.CurrentLogins = readEdgeLogin.CurrentLogins - 1;
            //}
            readEdgeLogin.LoginAllowed = true;
            readEdgeLogin.CurrentLogins = readEdgeLogin.CurrentLogins - 1;
            _prachiUser.UpdateReadEdgeLogin(readEdgeLogin);
            readEdgeUserLoginInfo.LogedOut = true;
            readEdgeUserLoginInfo.LogoutTime = DateTime.UtcNow.ToLocalTime();
            _prachiUser.UpdtaeReadEdgeUserLoginInfo(readEdgeUserLoginInfo);
         
          
     
        
            _HttpContextAccessor.HttpContext.Session.SetString("UserId", "");

            return RedirectToAction("Login");

        }
    }
}
       
