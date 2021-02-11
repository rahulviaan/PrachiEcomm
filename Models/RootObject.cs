using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    interface ICustomPrincipal : IPrincipal
    {
        string Id { get; set; }
        string Email { get; set; }
        string Username { get; set; }
        string Role { get; set; }
    }
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            var roles = GetRolesForUser(this.Id);
            return roles.Contains(role);
        }

        public CustomPrincipal(string email)
        {
            this.Identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public List<string> GetRolesForUser(string userId)
        {
            using (
                var userManager =
                    new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var rolesForUser = userManager.GetRoles(userId);

                return rolesForUser.ToList();
            }
        }
    }


    public class RootObject
    {
        public string Roles { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}