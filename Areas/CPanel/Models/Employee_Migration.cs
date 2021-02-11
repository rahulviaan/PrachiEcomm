using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class Employee_Migration : Book_Migration
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? new HttpContextWrapper(HttpContext.Current).GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public int MigrateEmployee(DataTable dt, int type)
        {

            var items = dt.AsEnumerable();
            foreach (var item in items)
            {
                var user = new ApplicationUser
                {
                    UserName = Convert.ToString(item["UserName"]),
                    Email = Convert.ToString(item["Email"]),
                    Address = Convert.ToString(item["Address"]),
                    FirstName = Convert.ToString(item["FirstName"]),
                    Country = Convert.ToString(item["Country"]),
                    State = Convert.ToString(item["State"]),
                    City = Convert.ToString(item["City"]),
                    PinCode = "",
                    PhoneNumber = Convert.ToString(item["PhoneNumber"]),
                    idServer = 0,
                    ProfileImage = "/UserProfileImage/Noimage.jpg"
                };
                var result = UserManager.Create(user, "Abc@123");
                if (result.Succeeded)
                {
                    var aspNetuser = UserManager.FindByEmail(user.UserName);
                    if (aspNetuser != null)
                    {
                        var IdentityRoleManager = new IdentityRoleManager();
                        if (!IdentityRoleManager.RoleExists(Roles.Sales))
                            IdentityRoleManager.CreateRole(Roles.Sales);
                        if (!IdentityRoleManager.RoleExists(Roles.Marketing))
                            IdentityRoleManager.CreateRole(Roles.Marketing);
                        if (type == 1)
                        {
                            IdentityRoleManager.AddUserToRole(aspNetuser.Id, Roles.Sales);

                        }
                        else
                        {
                            IdentityRoleManager.AddUserToRole(aspNetuser.Id, Roles.Marketing);
                        }
                    }
                  
                }

            }
            return 1;
        }
    }
}