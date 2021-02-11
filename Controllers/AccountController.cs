using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Sql.CustomRepositories;
using System.Collections.Generic;
using PrachiIndia.Sql;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Web.Security;
using MailServiece;
using PrachiIndia.Portal.Models;
using Fluentx.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
//using System.Web.Security;

namespace PrachiIndia.Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        AspNetUserRepository objAspNetUserRepository;
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        
        //
        // GET: /Account/Login


        [AllowAnonymous]
        public Action Test()
        {

            string js = "";
            //js = "[ {'CollectorDescription':'106440700345ddd0','CollectorOS':'Samsung SM-T311','CollectorType':'2','Domain':'prachi','FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormAnswer':[{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'2b159628 - fcea - 4a8d - 8341 - e721e19cebb7','IsAnswerText':'','OptionId':'2e3c72ca - abfe - 4d0c - b8c0 - e90073cddd4a','QuestionId':'b169cd82 - 186d - 47f3 - 9699 - fefc7958618a','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':1,'IsType':2,'IsAnswerInt':0},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'2969457e-ce80 - 48a2 - 8ba2 - 40e08447ec86','IsAnswerText':'','OptionId':'2dde80df - 5fe4 - 4413 - b174 - 48d2c20700eb','QuestionId':'edbddc25 - bca3 - 4149 - bb5f - 591fb4ff2783','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':2,'IsType':2,'IsAnswerInt':0},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'6a8b97b0 - ad72 - 43dd - 9088 - aa269d5f9c93','IsAnswerText':'','OptionId':'5ff36b24 - 5acb - 48b2 - 92c2 - 2d7ccf08e310','QuestionId':'235a0ad9 - 8389 - 4997 - bb9f - a793d7e18f55','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':3,'IsType':1,'IsAnswerInt':4},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'a4b99208 - c39e - 413c - a52c - ff4a5f41619b','IsAnswerText':'','OptionId':'624c8915 - e43b - 4581 - 9a2d - af4c6f7daff7','QuestionId':'235a0ad9 - 8389 - 4997 - bb9f - a793d7e18f55','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':4,'IsType':1,'IsAnswerInt':4},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'0f7e4797 - 8372 - 4355 - aada - dc81f0c3a01a','IsAnswerText':'','OptionId':'a980ede9 - 162b - 4b39 - 9373 - e335ab2887d7','QuestionId':'235a0ad9 - 8389 - 4997 - bb9f - a793d7e18f55','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':5,'IsType':1,'IsAnswerInt':4},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'2e505fad - 2840 - 44b9 - 8a6c - e3dae27b91f4','IsAnswerText':'','OptionId':'eac2c4e2 - 37ef - 447f - b2e9 - 125ccff33e35','QuestionId':'235a0ad9 - 8389 - 4997 - bb9f - a793d7e18f55','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':6,'IsType':1,'IsAnswerInt':3},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','FormPageId':'92978d06 - b304 - 4207 - 9f03 - 89c4b9619e4b','uuid':'9d8140ac - 0532 - 492c - b66b - 6d4a4f98eb77','IsAnswerText':'','OptionId':'ff6a4da6 - 3d06 - 480d - a5df - 502e37d095a8','QuestionId':'235a0ad9 - 8389 - 4997 - bb9f - a793d7e18f55','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':7,'IsType':1,'IsAnswerInt':3},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','uuid':'c94ff2f7 - 3dd2 - 4446 - 998d - d3cceda4feac','IsAnswerText':'harendra','QuestionId':'a6e9ac39 - d864 - 4c01 - b0ce - 01cfb72b9204','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':8,'IsType':4,'IsAnswerInt':0},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','uuid':'8172ac5c - 2b6b - 42b1 - a6c7 - 941dc050f471','IsAnswerText':'singhlk95 @gmail.com','QuestionId':'6802aced - 1005 - 4285 - b5b0 - cf380a742590','dtmCreate':'12 / 02 / 2016 06:34:37 AM','Status':1,'id':9,'IsType':4,'IsAnswerInt':0},{'FolderId':'32f7673a - ecf6 - 4efd - 949d - b0853067e7da','FormId':'502e0f28 - 03df - 43d2 - b84c - 6d9d17210790','uuid':'c15cd0b1 - 32cc - 4474 - bcc0 - 9fef72c3e96f','IsAnswerText':' +%_/^*(*(-\":;;!?,,.','QuestionId':'d60dd823-07af-4da6-bc15-92341709e3be','dtmCreate':'12/02/2016 06:34:37 AM','Status':1,'id':10,'IsType':5,'IsAnswerInt':0}],'FormId':'502e0f28-03df-43d2-b84c-6d9d17210790','Host':'192.168.2.11','HostLattitude':'0.0','HostLongitude':'0.0','Phone':'','comments':'','dtmCreate':'12/02/2016 06:34:37 AM','dtmSync':'12/02/2016 06:35:03 AM','isCall':'0','isHappy':'0','uuid':'543b3e72-4c24-43cb-bcce-79f1d7ad9f60'}]";
            //js = " { 'isName' : 'Abhishek','isUserName' : 'aa ','isPassword' : '12','isLongitude' : '23.9090','isLatitude' : '45.90980' ,'isRemark' : 'test','isDeviceInfo' : 'Empty','isStatus' : '1' } ";

            js = "[ {'UserName' : 'prachi@outlook.com',";
            js += "    'Email' : 'prachi@outlook.com',";
            js += "'FirstName' : 'AAAA BBB CCCC',";
            js += "'Address' : 'model.Address Prachi india PVT LTD',";
            js += "'Country' : '1',";
            js += "'State' : '1',";
            js += "'City' : '1',";
            js += "'PinCode' : '111111',";
            js += "'PhoneNumber' : '7666666666',";
            js += "'idServer' : '0'";
            js += "'Description' : 'model.Description  teasfdsfdsf  sfdsf dsf',";
            js += "'MachineKey' :'34343444343434343434',";
            js += " 'DeviceType' = '1'";
            js += "}]";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";

            string response = cli.UploadString("http://localhost:62906/Account/RegisterApp", "POST", js);
            return null;
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //  var pass = Encryption.DecryptCommon("uP+tQ95OycADCLlIpxdBQg==");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            objAspNetUserRepository = new AspNetUserRepository();

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var result = SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {

                case SignInStatus.Success:
                    {
                        var userMa = UserManager.FindByName(model.Email);
                        var roles = UserManager.GetRoles(userMa.Id).ToList();
                        var role = "";
                        if (roles != null)
                        {
                            foreach (var r in roles)
                            {
                                if (string.IsNullOrWhiteSpace(role))
                                    role = r;
                                else
                                    role = role + "," + r;
                            }
                        }

                        var RootObject = new RootObject
                        {
                            Email = userMa.Email,
                            Id = userMa.Id,
                            Roles = role,
                            UserName = userMa.UserName
                        };
                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(RootObject);
                        var authTicket = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData, FormsAuthentication.FormsCookiePath);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        HttpContext.Response.Cookies.Add(cookie);
                        var user = objAspNetUserRepository.SearchFor(u => u.UserName == model.Email).Select(v => new { ImageUrl = v.ProfileImage }).FirstOrDefault();
                        if (user == null)
                        {
                            Session["ProfileImageUrl"] = "/UserProfileImage/Noimage.jpg";
                        }
                        else
                        {
                            Session["ProfileImageUrl"] = user.ImageUrl;
                        }
                        var userRoles = IsAdmin(model.Email);
                        if (userRoles.Contains(PrachiIndia.Portal.Models.Roles.Admin) || userRoles.Contains(PrachiIndia.Portal.Models.Roles.SuperAdmin))
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "CPanel" });
                        }
                        else if (userRoles.Contains(PrachiIndia.Portal.Models.Roles.Sales))
                        {
                            return RedirectToAction("Index", "Sales", new { area = "Sales" });
                        }
                        else if (userRoles.Contains(PrachiIndia.Portal.Models.Roles.Marketing))
                        {
                            return RedirectToAction("Index", "Marketing", new { area = "Marketing" });
                        }
                        else
                        {
                            return RedirectToLocal(returnUrl, model.Email);
                        }

                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl, "");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var aspNetuser = new AspNetUserRepository().Get();
            //var rolemanager = new IdentityRoleManager();
            //var roles=rolemanager.GetAllRoles();
            var list = GetAllCountry();
            var State = GetSates();
            var City = GetCities();
            ViewData["list"] = list;
            ViewData["State"] = State;
            ViewData["City"] = City;
            //ViewBag.Roles = roles.Where(x=> !x.Name.ToLower().Contains("admin"));
            return View();
        }
        //
        //
        // POST: /Account/Register
        //[Route("Account/Register/{model}")]
        [AllowAnonymous]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var country = new CountryRepositories().FindByItemId(model.CounteryId);
            dynamic state = null;
            dynamic city = null;

            if (country != null && country.CountryId == 86)
            {
                state = new StateRepositories().FindByItemId(model.StateId);
                city = new CityRepositories().FindByItemId(model.CityId);
            }
            else
            {
                state = null;
                city = null;
            }


            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                FirstName = model.Name,
                CountryId = model.CounteryId,
                Country = country != null ? country.Name : string.Empty,
                StateId = model.StateId,
                State = state != null ? state.StateName : string.Empty,
                CityId = model.CityId,
                City = city != null ? city.CityName : string.Empty,
                PinCode = model.Pincode,
                PhoneNumber = model.PhoneNumber,
                idServer = model.idServer,
                ProfileImage = "/UserProfileImage/Noimage.jpg",
                dtmAdd = DateTime.Now
            };
            if (country != null && country.CountryId != 86 && country.CountryId > 0)
            {
                user.State = model.State;
                user.City = model.City;
            }

            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                CheckAndCreateRoles();
                var aspNetuser = new dbPrachiIndia_PortalEntities().AspNetUsers.FirstOrDefault(t => t.UserName == user.UserName);
                if (aspNetuser != null)
                {
                    UserManager.AddToRole(aspNetuser.Id, Models.Roles.User);
                }
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);


                //Create User Reader Abhishek Singh 04/05/2017
                if (!string.IsNullOrEmpty(model.MachineKey))
                {
                    var readerRepository = new ReaderRepository();
                    var UserReader = new Sql.UserReader()
                    {

                        UserId = aspNetuser.Id,
                        ReaderKey = Guid.NewGuid().ToString(),
                        Description = model.Description,
                        Status = (int)Web.Areas.Model.Status.Active,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        MachineKey = model.MachineKey,
                        DeviceType = (int)model.devType,
                        idServer = 0,

                    };
                    readerRepository.CreateAsync(UserReader);
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                if (model.SubscriptionId != null)
                {
                    TempData["SuscriptionId"] = model.SubscriptionId;
                    return RedirectToAction("_EditAdress", "Subscriptions");
                }
                return RedirectToAction("Index", "Catalogue");
            }
            AddErrors(result);
            var list = GetAllCountry();
            var State = GetSates();
            var City = GetCities();
            ViewData["list"] = list;
            ViewData["State"] = State;
            ViewData["City"] = City;
            // If we got this far, something failed, redisplay form
            return View(model);
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
            list.Insert(0, new SelectListItem { Text = "-Select Country-", Value = "0" });
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
        List<string> IsAdmin(string userNmae = "")
        {
            var user = UserManager.FindByName(userNmae);
            var userId = "";
            if (user != null)
            {
                userId = user.Id;
                return UserManager.GetRoles(userId).ToList();

            }
            return null;
        }
        [AllowAnonymous]
        public ActionResult doesUserPasswordExist(string OldPassword, string Email)
        {
            var userid = User.Identity.GetUserId();
            var user = UserManager.FindById(userid);
            var ispassword = UserManager.CheckPassword(user, OldPassword);
            bool ifEmailExist = ispassword;
            try
            {
                ifEmailExist = ispassword ? false : true;
                return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        /// <summary>
        /// Rahul Srivastava
        /// 28/08/2017
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>ActionResult</returns>
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
            if (!roleManager.RoleExists(Models.Roles.User))
            {
                roleManager.CreateRole(Models.Roles.User);
            }
            if (!roleManager.RoleExists(Models.Roles.Teacher))
            {
                roleManager.CreateRole(Models.Roles.Teacher);
            }
            if (!roleManager.RoleExists(Models.Roles.School))
            {
                roleManager.CreateRole(Models.Roles.School);
            }
            if (!roleManager.RoleExists(Models.Roles.Admin))
            {
                roleManager.CreateRole(Models.Roles.Admin);
            }
            if (!roleManager.RoleExists(Models.Roles.SuperAdmin))
            {
                roleManager.CreateRole(Models.Roles.SuperAdmin);
            }
            if (!roleManager.RoleExists(Models.Roles.Sales))
            {
                roleManager.CreateRole(Models.Roles.Sales);
            }
            if (!roleManager.RoleExists(Models.Roles.Marketing))
            {
                roleManager.CreateRole(Models.Roles.Marketing);
            }
            if (!roleManager.RoleExists(Models.Roles.Staff))
            {
                roleManager.CreateRole(Models.Roles.Staff);
            }
            if (!roleManager.RoleExists(Models.Roles.Driver))
            {
                roleManager.CreateRole(Models.Roles.Driver);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = UserManager.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                //For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //Send an email with this link
                string code = UserManager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, protocol: Request.Url.Scheme);
                // UserManager.SendEmail(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                Mail.SendMail(user.Email, "ResetPassword", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        /* 

        */
        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.FindByName(model.Email);
            //var user = await UserManager.(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = UserManager.ResetPassword(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = SignInManager.GetVerifiedUserId();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = UserManager.GetValidTwoFactorProviders(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!SignInManager.SendTwoFactorCode(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = SignInManager.ExternalSignIn(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl, "");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl, "");
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            FormsAuthentication.SignOut();
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MielibReader()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //FormsAuthentication.SignOut();
            //// Clear authentication cookie.
            //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //cookie.Expires = DateTime.Now.AddYears(-1);
            //Response.Cookies.Add(cookie);
            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("MieLibId", User.Identity.GetUserId());
            return this.RedirectAndPost("http://reader.mielib.com/Home/ViewerDashboard", postData);
//            return Json(new { result = "ok" },JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #region User Profile Work start here
        public ActionResult UserProfile()
        {
            var AspnetRepository = new AspNetUserRepository();
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            IQueryable<AspNetUser> query = AspnetRepository.GetAll();
            var RepoShipping = new ShippingAddressRepository();
            IQueryable<TblShipingAddress> querys = RepoShipping.GetAll();

            var userId = User.Identity.GetUserId();
            var ists = querys.Where(s => s.UserId == userId).ToList();
            var list = query.Where(s => s.Id == userId).ToList();
            List<AspNetUser> listsss = new List<AspNetUser>();
            foreach (var models in ists)
            {
                var Cid = Convert.ToInt32(models.Country != null ? models.Country : 0);
                var Sid = Convert.ToInt32(models.State != null ? models.State : 0);
                var CiId = Convert.ToInt32(models.City != null ? models.City : 0);
                //var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";//Convert.ToInt32(model.Country != null ? model.Country:"0")).Name;
                //var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";//Convert.ToInt32(model.State!=null?model.State:"0")).StateName;
                //var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";//Convert.ToInt32(model.City != null ? model.City:"0")).CityName;
                var user = new AspNetUser
                {
                    UserName = models.UserName,
                    Email = models.Email,
                    Address = models.Address,
                    FirstName = models.UserName,
                    Country = models.Countries,
                    State = models.States,
                    City = models.Cities,
                    CountryId = Convert.ToInt32(Cid),
                    StateId = Convert.ToInt32(Sid),
                    CityId = Convert.ToInt32(CiId),
                    PinCode = models.PinCode,
                    PhoneNumber = models.Phone
                };
                listsss.Add(user);

                ViewData["Shipping"] = listsss;
            }
            List<AspNetUser> listss = new List<AspNetUser>();
            foreach (var model in list)
            {
                var Cid = model.CountryId ?? 0;
                var Sid = model.StateId ?? 0;
                var CiId = model.CityId ?? 0;
                //var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";//Convert.ToInt32(model.Country != null ? model.Country:"0")).Name;
                //var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";//Convert.ToInt32(model.State!=null?model.State:"0")).StateName;
                //var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";//Convert.ToInt32(model.City != null ? model.City:"0")).CityName;
                var user = new AspNetUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Address = model.Address,
                    FirstName = model.FirstName,
                    Country = model.Country,
                    State = model.State,
                    City = model.City,
                    CountryId = Convert.ToInt32(Cid),
                    StateId = Convert.ToInt32(Sid),
                    CityId = Convert.ToInt32(CiId),
                    PinCode = model.PinCode,
                    PhoneNumber = model.PhoneNumber,
                    ProfileImage = model.ProfileImage
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
        public ActionResult _EditUserProfile(AspNetUser model, HttpPostedFileBase UserImage)
        {
            var aspNetUserRepository = new AspNetUserRepository();
            int result = 0;
            try
            {
                AspNetUser users = new AspNetUser();
                var user = new AspNetUserRepository().GetUser(User.Identity.GetUserId());
                string ImageLocation = string.Empty;
                if (UserImage != null)
                {
                    if (UserImage.ContentLength > 0)
                    {
                        string filePath = HttpContext.Server.MapPath("~/UserProfileImage/" + User.Identity.GetUserId() + Path.GetExtension(UserImage.FileName));
                        ImageLocation = "/UserProfileImage/" + User.Identity.GetUserId() + Path.GetExtension(UserImage.FileName);
                        UserImage.SaveAs(filePath);
                        //Added by Rahul Srivastava on 04/10/2017 to update the image 
                        Session["ProfileImageUrl"] = ImageLocation;
                    }
                }
                else
                {
                    objAspNetUserRepository = new AspNetUserRepository();
                    var userImage = objAspNetUserRepository.SearchFor(u => u.UserName == user.UserName).Select(v => new { ImageUrl = v.ProfileImage }).FirstOrDefault();
                    if (userImage == null)
                    {
                        Session["ProfileImageUrl"] = "/UserProfileImage/Noimage.jpg";
                        ImageLocation = "/UserProfileImage/Noimage.jpg";
                    }
                    else
                    {
                        Session["ProfileImageUrl"] = userImage.ImageUrl;
                        ImageLocation = userImage.ImageUrl;
                    }
                }

                users.Id = User.Identity.GetUserId();
                users.FirstName = model.FirstName;
                users.Email = model.Email;
                users.PhoneNumber = model.PhoneNumber;
                users.PinCode = model.PinCode;
                users.Country = model.CountryId.ToString();
                users.State = model.StateId.ToString();
                users.City = model.CityId.ToString();
                users.Address = model.Address;
                users.ProfileImage = ImageLocation;
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
        [HttpPost]
        public ActionResult _EditUserBilling(AspNetUser model)
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
                users.Country = model.CountryId.ToString();
                users.State = model.StateId.ToString();
                users.City = model.CityId.ToString();
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
        [HttpPost]
        public async Task<ActionResult> _EditUserShipping(AspNetUser model)
        {
            var repo = new ShippingAddressRepository();
            var userid = User.Identity.GetUserId();
            var username = User.Identity.GetUserName();
            var user = repo.GetUsers(User.Identity.GetUserId());
            if (user == null)
            {
                TblShipingAddress shipping = new TblShipingAddress();
                shipping.UserName = model.FirstName;
                shipping.UserId = userid;
                shipping.PinCode = model.PinCode;
                shipping.Email = model.Email;
                shipping.Phone = model.PhoneNumber;
                shipping.Country = Utility.ToSafeInt(model.CountryId);
                shipping.State = Utility.ToSafeInt(model.StateId);
                shipping.City = Utility.ToSafeInt(model.CityId);
                shipping.Address = model.Address;
                repo.CreateAsync(shipping);

            }
            else
            {
                try
                {
                    AspNetUser users = new AspNetUser();

                    var shippings = new ShippingAddressRepository().GetUsers(User.Identity.GetUserId());
                    TblShipingAddress shipping = new TblShipingAddress();
                    // shipping.id = shippings.id;
                    shippings.UserName = model.FirstName;
                    shippings.UserId = userid;
                    shippings.PinCode = model.PinCode;
                    shippings.Email = model.Email;
                    shippings.Phone = model.PhoneNumber;
                    shippings.Country = Utility.ToSafeInt(model.CountryId);
                    shippings.State = Utility.ToSafeInt(model.StateId);
                    shippings.City = Utility.ToSafeInt(model.CityId);
                    shippings.Address = model.Address;
                    var result = repo.Update(shippings);
                }
                catch (Exception ex)
                {
                }
                //if (result >= 0)
                //{
                //    var user = new AspNetUserRepository().FindById(User.Identity.GetUserId());
                //    foreach (var item in user)
                //    {
                //        model.FirstName = item.FirstName;
                //        model.Email = item.Email;
                //        model.Country = item.Country;
                //        model.City = item.City;
                //        model.PinCode = item.PinCode;
                //        model.PhoneNumber = item.PhoneNumber;
                //        model.Address = item.Address;
                //    }

                //}
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
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
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
                var result = await UserManager.ResetPasswordAsync(UserId, code, model.Password);
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

        private ActionResult RedirectToLocal(string returnUrl, string userNmae)
        {
            var roles = IsAdmin(userNmae);
            if (roles.Contains(PrachiIndia.Portal.Models.Roles.Admin) || roles.Contains(PrachiIndia.Portal.Models.Roles.SuperAdmin))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "CPanel" });
            }
            else if (roles.Contains(PrachiIndia.Portal.Models.Roles.Sales) || roles.Contains(PrachiIndia.Portal.Models.Roles.Marketing))
            {
                return RedirectToAction("Index", "Sales", new { area = "" });
            }
            else
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                string[] strArr = null;
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        strArr = returnUrl.Split(',');
                    }
                    if (strArr[0] == "Catalogue")
                    {
                        TempData["BookId"] = strArr[1];
                        //string BookId = strArr[1];
                        return RedirectToAction("Index", "Catalogue");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AccountSetting()
        {
            var AspnetRepository = new AspNetUserRepository();
            IQueryable<AspNetUser> query = AspnetRepository.GetAll();
            var userId = User.Identity.GetUserId();
            var list = query.Where(s => s.Id == userId).ToList();
            ViewData["Profile"] = list;
            return View();
        }
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}