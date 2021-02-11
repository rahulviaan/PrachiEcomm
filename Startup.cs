using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrachiIndia.Portal.Startup))]
namespace PrachiIndia.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
