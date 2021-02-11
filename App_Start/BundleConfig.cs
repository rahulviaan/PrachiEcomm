using System.Web;
using System.Web.Optimization;

namespace PrachiIndia.Portal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/toaster").Include(
                       "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryUnobtrusive").Include(
                       "~/Scripts/jquery.unobtrusive-ajax*"));

            bundles.Add(new StyleBundle("~/toasterCss").Include(
                                "~/Content/Dashboard/css/toastr.min.css"));
          // Use the development version of Modernizr to develop with and learn from. Then, when you're
          // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
          bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
             "~/Content/Dashboard/js/jquery.js",
             "~/Content/Dashboard/js/jquery-ui-1.10.4.min.js",
             "~/Content/Dashboard/js/jquery-1.8.3.min.js",
             "~/Content/Dashboard/js/bootstrap.min.js",
             "~/Content/Dashboard/js/jquery.scrollTo.min.js",
             "~/Content/Dashboard/js/jquery.nicescroll.js",
             "~/Content/Dashboard/js/scripts.js"
            ));
            bundles.Add(new StyleBundle("~/Dashboard/css").Include(
                "~/Content/Dashboard/css/bootstrap.min.css",
                "~/Content/Dashboard/css/bootstrap.css",
                "~/Content/Dashboard/css/elegant-icons-style.css",
                "~/Content/Dashboard/css/font-awesome.min.css",
                "~/Content/Dashboard/css/style.css",
                "~/Content/Dashboard/css/style-responsive.css"
            ));
            bundles.Add(new StyleBundle("~/font/css").Include(
                 "~/Content/font-awesome.css"
                ));
        }
    }
}
