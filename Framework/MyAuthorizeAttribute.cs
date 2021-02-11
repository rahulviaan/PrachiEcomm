using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Framework
{
    public class MyAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Authenticate.IsAuthenticated() && httpContext.User.Identity.IsAuthenticated)
            {
                var authCookie = httpContext.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                string[] roles = null;

                if (authCookie != null)
                {
                    var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                    roles = ticket.UserData.Split('|');
                    var identity = new GenericIdentity(ticket.Name);
                    httpContext.User = new GenericPrincipal(identity, roles);
                }

                if (Roles == string.Empty)
                    return true;

                //Assuming Roles given in the MyAuthorize attribute will only have 1 UserAccountType - if more than one, no errors thrown but will always return false
                else if ((UserAccountType)Enum.Parse(typeof(UserAccountType), roles[0]) >= (UserAccountType)Enum.Parse(typeof(UserAccountType), Roles))
                    return true;
                else
                    return false;
            }
            else
                return false;

            //return base.AuthorizeCore(httpContext);
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!Authenticate.IsAuthenticated())
                HandleUnauthorizedRequest(filterContext);

            base.OnAuthorization(filterContext);
        }
    }
}