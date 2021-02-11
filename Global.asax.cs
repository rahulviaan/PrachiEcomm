using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using PrachiIndia.Sql;
using PrachiIndia.Portal.Helpers;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Script.Serialization;
using ExceptionLogger;
using PrachiException;
using System.Configuration;
using System.Globalization;
using System.Threading;
using PrachiIndia.Portal.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PrachiIndia.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Database.SetInitializer<ApplicationDbContext>(null);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Task t = Task.Run(() =>
            {
                Utility.LoadCatalogue();
            });
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Added by Rahul on 01/09/2017  
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            //objtblCataLog =context.tblCataLogs.ToList<tblCataLog>();

            //objtblCataLog = objtblCataLog.OrderBy(x => Convert.ToInt32(x.ClassId)).ToList();
            PrachiIndia.Portal.Helpers.Utility.LoadCatalogue();

        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;

            //if (!Context.Request.IsSecureConnection)
            //    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));

            //if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
            //{
            //    UriBuilder builder = new UriBuilder(Request.Url);
            //    builder.Host = "www." + Request.Url.Host;
            //    Response.StatusCode = 301;
            //    Response.AddHeader("Location", builder.ToString());
            //    Response.End();
            //}
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    //var roles = authTicket.UserData.Split(new Char[] { ',' });
                    //var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                    //Context.User = userPrincipal;

                    var serializer = new JavaScriptSerializer();
                    var serializeModel = serializer.Deserialize<RootObject>(authTicket.UserData);

                    CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                    newUser.Id = serializeModel.Id;
                    newUser.Username = serializeModel.UserName;
                    newUser.Email = serializeModel.Email;
                    // newUser.Role = string.Empty;

                    HttpContext.Current.User = newUser;
                }
                catch
                {
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(cookie);
                }
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (Server.GetLastError() != null)
            {
                //handling errors globally, better to have use it in place of having multiple 
                //try catch on each page if not necessarry...[Rahul srivastava]
                try
                {
                    Exception ex = Server.GetLastError();

                    if (ex != null)
                    {
                        dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
                        string provider = Convert.ToString(ConfigurationManager.AppSettings["ErroLog"]);
                        Prachi_Exception PrachiExceptionObject = new Prachi_Exception();
                        PrachiExceptionObject.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        PrachiExceptionObject.EnableSSL = true;
                        PrachiExceptionObject.LogProvider = provider;
                        PrachiExceptionObject.Exception = ex;
                        PrachiExceptionObject.Subject = "Prachi Excepction";
                        PrachiExceptionObject.ProjectName = "Prachi";
                        PrachiExceptionObject.ToList = string.Join(",", context.DeveloperMasters.Where(p => p.Status == true).Select(d => d.EmailId.ToString()));
                        Logger.Log(PrachiExceptionObject);
                        Server.ClearError();
                        Response.Redirect("~/Home/Excepction");
                    }
                    else
                    {
                        Server.ClearError();
                    }
                }
                catch (Exception ex)
                {


                }

            }
        }
    }
}
