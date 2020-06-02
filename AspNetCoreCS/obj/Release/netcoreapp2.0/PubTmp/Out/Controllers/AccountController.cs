using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public AccountController(IUser user, IPrachiuser prachiUser, IHttpContextAccessor httpContextAccessor)
        {
            _User = user;
            _prachiUser = prachiUser;
            _HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult Login()
        {
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
                    var role= _prachiUser.GetUserRole(result.Result.Id);
                if (role == null)
                {
                    ViewBag.Message = "You are not subscribed user !!!";
                    return View();
                }

                _HttpContextAccessor.HttpContext.Session.Remove("Role");
                _HttpContextAccessor.HttpContext.Session.SetString("Role", role.Name);
                //var roleid=_prachiUser.GetUserRoleId(result.Result.Id);
                _HttpContextAccessor.HttpContext.Session.Remove("UserId");
                    _HttpContextAccessor.HttpContext.Session.SetString("UserId", result.Result.Id);
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ViewBag.Message = "Invalid login attempt !!!";
                    return View();
                }
            }
    
        
        public IActionResult logoff()
        {
            _HttpContextAccessor.HttpContext.Session.SetString("UserId", "");
            return RedirectToAction("Login");

        }
    }
}
       
