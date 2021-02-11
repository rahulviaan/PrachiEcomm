using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class UserdetailController : Controller
    {
        // GET: CPanel/Userdetail
        private ApplicationUserManager _userManager;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserProfile()
        {
            var AspnetRepository = new AspNetUserRepository();
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            IQueryable<AspNetUser> query = AspnetRepository.GetAll();
            var userId = User.Identity.GetUserId();
            var list = query.Where(s => s.Id == userId).ToList();
            List<AspNetUser> listss = new List<AspNetUser>();
            foreach (var model in list)
            {
                var Cid = Convert.ToInt32(model.Country != null ? model.Country : "0");
                var Sid = Convert.ToInt32(model.State != null ? model.State : "0");
                var CiId = Convert.ToInt32(model.City != null ? model.City : "0");
                var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";//Convert.ToInt32(model.Country != null ? model.Country:"0")).Name;
                var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";//Convert.ToInt32(model.State!=null?model.State:"0")).StateName;
                var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";//Convert.ToInt32(model.City != null ? model.City:"0")).CityName;
                var user = new AspNetUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Address = model.Address,
                    FirstName = model.FirstName,
                    Country = countryid,
                    State = StateId,
                    City = CityId,
                    CountryId = Convert.ToInt32(Cid),
                    StateId = Convert.ToInt32(Sid),
                    CityId = Convert.ToInt32(CiId),
                    PinCode = model.PinCode,
                    PhoneNumber = model.PhoneNumber
                };
                listss.Add(user);
                ViewData["Profile"] = listss;
            }
            var lists = GetAllCountry();
            var State = GetSates();
            var City = GetCities();
            ViewData["list"] = lists;
            ViewData["State"] = State;
            ViewData["City"] = City;
            return View();
        }
        [HttpPost]
        public ActionResult _EditUserProfile(AspNetUser model)
        {
            var aspNetUserRepository = new AspNetUserRepository();
            int result = 0;
            try
            {
                AspNetUser users = new AspNetUser();
                var user = new AspNetUserRepository().GetUser(User.Identity.GetUserId());
                users.Id = User.Identity.GetUserId();
                users.FirstName = model.FirstName;
                users.Email = model.Email;
                users.PhoneNumber = model.PhoneNumber;
                users.PinCode = model.PinCode;
                users.Country = model.Country.ToString();
                users.State = model.State.ToString();
                users.City = model.City.ToString();
                users.Address = model.Address;
                result = aspNetUserRepository.updateUser(users);
            }
            catch (Exception ex)
            {
            }
            if (result >= 0)
            {
                var user = new AspNetUserRepository().FindById(User.Identity.GetUserId());
                foreach (var item in user)
                {
                    model.FirstName = item.FirstName;
                    model.Email = item.Email;
                    model.Country = item.Country;
                    model.City = item.City;
                    model.PinCode = item.PinCode;
                    model.PhoneNumber = item.PhoneNumber;
                    model.Address = item.Address;
                }

            }
            var list = GetAllCountry();
            var State = GetSates();
            var City = GetCities();
            ViewData["list"] = list;
            ViewData["State"] = State;
            ViewData["City"] = City;
            ViewData["userdetail"] = list;
            return Redirect("UserProfile");
        }
        private object GetAllCountry()
        {
            var CountryRepo = new CountryRepositories();
            var CountryList = CountryRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            foreach (var item in CountryList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.Name, Value = item.CountryId.ToString() });
            }
            return list;
        }
        [AllowAnonymous]
        public object GetSates()
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in StateList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.StateName, Value = item.StaeteId.ToString() });
            }
            return list;
        }

        [AllowAnonymous]
        public object GetCities()
        {
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in CityList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.CityName, Value = item.CityId.ToString() });
            }
            return list;
        }
        [AllowAnonymous]
        public JsonResult GetCity(string StateId)
        {
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            var district = from s in CityList.ToList()
                           where s.StateId == Convert.ToInt32(StateId)
                           select s;
            return Json(new SelectList(district.ToArray(), "CityId", "CityName"), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetSate(string id)
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            var district = from s in StateList.ToList()
                           where s.CountryId == Convert.ToInt32(id)
                           select s;
            return Json(new SelectList(district.ToArray(), "StaeteId", "StateName"), JsonRequestBehavior.AllowGet);
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(RegisterViewModel model)
        {
            string massage = "";
            try
            {
                var UserId = User.Identity.GetUserId();
                string code = await UserManager.GeneratePasswordResetTokenAsync(UserId);
                var result = await UserManager.ResetPasswordAsync(UserId, code, model.newPassword);
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                if (result.Succeeded)
                {
                    massage = "password Chage";
                    return RedirectToAction("UserProfile");
                }
                else
                {
                    massage = "pasword not change";
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ChangePassword");
            }
        }
        /// <summary>
        /// Rahul Srivastava
        /// UserdetailController
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Boolen</returns>
        public bool EmailAvailable(string Email)
        {
            AspNetUserRepository obj = new AspNetUserRepository();
            return obj.IsAvailable(x => x.Email == Email);
        }

        public ActionResult ManageUsers()
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            var states = (from s in StateList
                          where s.IsActive == true
                          select new SelectListItem
                          {
                              Value = s.StaeteId.ToString(),
                              Text = s.StateName
                          }).ToList();
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            var district = (from s in CityList.ToList()
                            where s.IsActive == true
                            select new SelectListItem
                            {
                                Value = s.CityId.ToString(),
                                Text = s.CityName
                            }).ToList();
            return View();
        }
    }
}