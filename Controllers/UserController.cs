using PrachiIndia.Portal.Helpers;
using System;
using System.Web.Http;
using PrachiIndia.Sql.CustomRepositories;
using System.Linq;
using PrachiIndia.Sql;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Controllers
{
    [Authorize(Roles = "User,School")]
    public class UserController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }




        [AllowAnonymous]
        [HttpPost]
        [Route("api/PostUser")]
        public async Task<RegisterViewModel> PostUser([FromBody]RegisterViewModel model)
        {

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    FirstName = model.Name,
                    Country = model.Country,
                    State = model.State,
                    City = model.City,
                    PinCode = model.Pincode,
                    PhoneNumber = model.PhoneNumber,
                    idServer = model.idServer
                };

                var AspUserRepo = new AspNetUserRepository();
                IQueryable<AspNetUser> users = AspUserRepo.SearchFor(p => p.Email == user.Email || p.UserName == user.UserName);
                if (users.Count() > 0)
                {
                    return null;
                }
                else
                {
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        AccountController.CheckAndCreateRoles();
                        var aspNetuser = new dbPrachiIndia_PortalEntities().AspNetUsers.FirstOrDefault(t => t.UserName == user.UserName);
                        if (aspNetuser != null)
                        {
                            var v1 = await UserManager.AddToRoleAsync(aspNetuser.Id, model.Role);
                        }
                        var readerRepository = new ReaderRepository();
                        var ReaderKey = Guid.NewGuid().ToString();
                        var UserReader = new Sql.UserReader()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = aspNetuser.Id,
                            ReaderKey = ReaderKey,
                            Description = model.Description,
                            Status = (int)Web.Areas.Model.Status.Active,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            MachineKey = model.MachineKey,
                            DeviceType = (int)model.devType,
                            BookVersion = 1,
                            MasterVesrion = 1,
                            LibraryVersion = 1,
                            SynchDate = DateTime.UtcNow,
                            idServer = 0,
                        };
                         readerRepository.CreateAsync(UserReader);
                        model.Id = aspNetuser.Id;
                        model.ReaderId = UserReader.Id;
                        model.ReaderKey = ReaderKey;
                    }
                    return model;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/UpdateUser")]
        public async Task<RegisterViewModel> UpdateUser([FromBody]RegisterViewModel model)
        {
            try
            { 
                var AspUserRepo = new AspNetUserRepository();
                var user = AspUserRepo.GetUser(model.Id);
                if (user != null)
                {
                     
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.FirstName = model.Name;
                    user.PhoneNumber = model.PhoneNumber;
                    var result=new AspNetUserRepository().UserUpdate(user);
                    return model;
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/UserExist")]
        public async Task<bool> UserExist([FromBody]RegisterViewModel user)
        {

            try
            {
                var AspUserRepo = new AspNetUserRepository();
                IQueryable<AspNetUser> users = AspUserRepo.SearchFor
                    (
                        p => p.Email == user.Email &&
                        p.UserName == user.Email &&
                        p.FirstName.ToUpper() == user.Name.ToUpper() &&
                        p.PhoneNumber == user.PhoneNumber
                    );
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/UserAvailable")]
        public async Task<bool> UserAvailable([FromBody]RegisterViewModel user)
        {
            try
            {
                var AspUserRepo = new AspNetUserRepository();
                IQueryable<AspNetUser> users = AspUserRepo.SearchFor
                    (
                        p =>  p.UserName.ToLower().Trim() == user.Email.ToLower().Trim() 
                       
                    );
                if (users.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("api/UpdatePassword")]
        public async Task<bool> UpdatePassword([FromBody]RegisterViewModel model)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(model.Id))
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    var v1 = await UserManager.RemovePasswordAsync(user.Id);
                    var v2 = await UserManager.AddPasswordAsync(user.Id, model.Password);
                    return true;

                }
              else
                {
                    var v1 = await UserManager.RemovePasswordAsync(model.Id);
                    var v2 = await UserManager.AddPasswordAsync(model.Id, model.Password);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
