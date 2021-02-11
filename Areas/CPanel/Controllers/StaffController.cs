using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    public class StaffController : Controller
    {
        private ApplicationUserManager _userManager;
        // GET: CPanel/Staff
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
        public ActionResult Index(string type = "", long StateId = 0, long CityId = 0)
        {
            var context = new dbPrachiIndia_PortalEntities();
            var users = context.AspNetUsers.AsQueryable();
            if (StateId > 0)
            {
                users = users.Where(t => t.StateId == StateId);
            }
            if (CityId > 0)
            {
                users = users.Where(t => t.CityId == CityId);
            }
            dynamic items;
            if (string.IsNullOrWhiteSpace(type))
            {
                items = (from user in users
                         where user.AspNetRoles.Any(r => r.Name == PrachiIndia.Portal.Models.Roles.Sales || r.Name == PrachiIndia.Portal.Models.Roles.Marketing)
                         select user).OrderBy(t => t.FirstName).ToList();
            }
            else
            {
                items = (from user in users
                         where user.AspNetRoles.Any(r => r.Name == type)
                         select user).OrderBy(t => t.FirstName).ToList();
            }
            var State = GetSates();
            ViewBag.State = State;
            return View(items);
        }

        public ActionResult CreateStaff()
        {
            var State = GetSates();
            var City = GetCities();
            ViewData["State"] = State;
            ViewData["City"] = City;
            var roles = new List<System.Web.Mvc.SelectListItem>
            {
                new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Staff, Value=PrachiIndia.Portal.Models.Roles.Staff },
                new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Driver, Value=PrachiIndia.Portal.Models.Roles.Driver },
                new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Sales, Value=PrachiIndia.Portal.Models.Roles.Sales },
                 new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Marketing, Value=PrachiIndia.Portal.Models.Roles.Marketing },
            };
            ViewData["Roles"] = roles;
            return View();
        }
        [HttpPost]
        public ActionResult CreateStaff(StaffModel model)
        {
            if (!ModelState.IsValid)
            {
                var State = GetSates();
                var City = GetCities();
                ViewData["State"] = State;
                ViewData["City"] = City;
                var roles = new List<System.Web.Mvc.SelectListItem>
                    {
                        new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Staff, Value=PrachiIndia.Portal.Models.Roles.Staff },
                        new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Driver, Value=PrachiIndia.Portal.Models.Roles.Driver },
                        new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Sales, Value=PrachiIndia.Portal.Models.Roles.Sales },
                        new SelectListItem {Text=PrachiIndia.Portal.Models.Roles.Marketing, Value=PrachiIndia.Portal.Models.Roles.Marketing },
                    };
                ViewData["Roles"] = roles;
                return View(model);
            }
            dynamic state = new StateRepositories().FindByItemId(model.StateId);
            dynamic city = new CityRepositories().FindByItemId(model.CityId);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                FirstName = model.Name,
                CountryId = 86,
                Country = "India",
                StateId = model.StateId,
                State = state != null ? state.StateName : string.Empty,
                CityId = model.CityId,
                City = city != null ? city.CityName : string.Empty,
                PinCode = model.Pincode,
                PhoneNumber = model.PhoneNumber,
                ProfileImage = "/UserProfileImage/Noimage.jpg",
                Status = 1,
                CompleteName = model.Name,
                //VehicleNo = model.VehicleNo,
                //VehicleType = model.VehicleType
            };
            var result = UserManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                CheckAndCreateRoles();
                var aspNetuser = new dbPrachiIndia_PortalEntities().AspNetUsers.FirstOrDefault(t => t.UserName == user.UserName);
                if (aspNetuser != null)
                {
                    UserManager.AddToRole(aspNetuser.Id, model.Role);
                }
            }
            return RedirectToAction("Index", "Staff", new { type = model.Role, });


        }
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

        public JsonResult EmailExists(string Email)
        {
            AspNetUserRepository obj = new AspNetUserRepository();
            bool result = obj.IsAvailable(x => x.UserName == Email);
            try
            {
                //ifEmailExist = ispassword ? false : true;
                return Json(!result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public static void CheckAndCreateRoles()
        {
            var roleManager = new IdentityRoleManager();
            if (!roleManager.RoleExists(Roles.User))
            {
                roleManager.CreateRole(Roles.User);
            }
            if (!roleManager.RoleExists(Roles.Teacher))
            {
                roleManager.CreateRole(Roles.Teacher);
            }
            if (!roleManager.RoleExists(Roles.School))
            {
                roleManager.CreateRole(Roles.School);
            }
            if (!roleManager.RoleExists(Roles.Admin))
            {
                roleManager.CreateRole(Roles.Admin);
            }
            if (!roleManager.RoleExists(Roles.SuperAdmin))
            {
                roleManager.CreateRole(Roles.SuperAdmin);
            }
            if (!roleManager.RoleExists(Roles.Sales))
            {
                roleManager.CreateRole(Roles.Sales);
            }
            if (!roleManager.RoleExists(Roles.Marketing))
            {
                roleManager.CreateRole(Roles.Marketing);
            }
            if (!roleManager.RoleExists(Roles.Staff))
            {
                roleManager.CreateRole(Roles.Staff);
            }
            if (!roleManager.RoleExists(Roles.Driver))
            {
                roleManager.CreateRole(Roles.Driver);
            }
        }
    }
}