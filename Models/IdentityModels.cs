using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Collections.Generic;
using System;

namespace PrachiIndia.Portal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }


        public DateTime? dtmAdd { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }

        public int Status { get; set; }
        public int IsVerified { get; set; }

        public DateTime? dtmDob { get; set; }
        public string AboutMe { get; set; }
        public string ProfileImage { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string Industry { get; set; }
        public string PANId { get; set; }
        public string PassportNo { get; set; }
        public string DlNo { get; set; }
        public string Remark { get; set; }
        public long? idServer { get; set; }

        public string ReaderKey { get; set; }
        public string extra { get; set; }
        public int idextra { get; set; }
        public int idextra1 { get; set; }
        //public string VehicleType { get; set; }
        //public string VehicleNo { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    public class IdentityRoleManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }


        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }
        public string GetRole(string roleId)
        {
            var context = new ApplicationDbContext();
            var role = context.Roles.FirstOrDefault(t => t.Id == roleId);
            if (role != null)
                return role.Name;
            return "";
            //var rm = new RoleManager<IdentityRole>(
            //    new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //var idResult = rm.(new IdentityRole(name));
            //return idResult.Succeeded;
        }
        public List<IdentityRole> GetAllRoles()
        {
            var context = new ApplicationDbContext();
            var role = context.Roles.ToList();
            return role;
        }
        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //um.UserValidator = new UserValidator<ApplicationUser>(um)
            //{
            //   AllowOnlyAlphanumericUserNames = false,
            //};
            var idResult = um.Create(user, password);
            return idResult;
        }

        public bool DeleteUser(string userId)
        {
            var appDb = new ApplicationDbContext();
            var appUser = appDb.Users.FirstOrDefault(u => u.Id == userId);
            if (appUser != null)
            {
                appDb.Users.Remove(appUser);
                return appDb.SaveChanges() > 0;
            }
            return false;
        }
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.UserValidator = new UserValidator<ApplicationUser>(um)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            um.UserValidator = new UserValidator<ApplicationUser>(um)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            // currentRoles.AddRange(user.Roles.ToList());
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }



    }
    public static class Roles
    {
        public const string User = "User";
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string School = "School";
        public const string SuperAdmin = "SuperAdmin";
        public const string Marketing = "Marketing";
        public const string Sales = "Sales";
        public const string Staff = "Staff";
        public const string Driver = "Driver";
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var hashPassword = PrachiIndia.Portal.Helpers.Encryption.EncryptCommon(password);
            return hashPassword;
        }

        public PasswordVerificationResult VerifyHashedPassword
                      (string hashedPassword, string providedPassword)
        {
            return hashedPassword == HashPassword(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }

}