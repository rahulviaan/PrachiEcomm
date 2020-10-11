using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GleamTech.AspNet;
using GleamTech.AspNet.Core;
using GleamTech.DocumentUltimate;
using Microsoft.AspNetCore.Http.Features;
using DAL.Models.Interfaces;
using ReadEdgeCore.Models.Interfaces;
using DAL.Models.Entities;
using DAL.Models.Repository;
using ReadEdgeCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using ReadEdgeCore.Hubs;
using GleamTech.DocumentUltimate.AspNet;
using Microsoft.AspNetCore.Http;
using GleamTech.DocumentUltimateExamples.AspNetCoreCS.Filters;

namespace GleamTech.DocumentUltimateExamples.AspNetCoreCS
{
    public class Startup
    {
        //private IHttpContextAccessor _httpContextAccessor;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(12000);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddSignalR();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            //services.Configure<FormOptions>(options =>
            //{
            //    options.MultipartBodyLengthLimit = 1000000000;
            //});
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            services.AddMvc();
            services.AddGleamTech();

          //services.AddMvc(
          //config =>
          //{
          //    config.Filters.Add(new AsyncActionFilter());
          //});

            // Add ASPNETCoreDemoDBContext services.
            services.AddDbContext<UserContext>(options => options.UseSqlServer(Configuration.GetConnectionString("UserContext")));
            services.AddScoped<AsyncActionFilter>();
            services.AddScoped<IPrachiuser, PrachiUser>();
            services.AddScoped<IPrachiUserRepository, PrachiUserLoginRepository>();
            services.AddDbContext<ReadEdgeCoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReadEdgeCoreContext")));
            // services.AddDbContext<ReadEdgeCoreContext>(options => options.UseSqlite("Data Source=ReadedgeCore.db"));

            #region DI Regeistration
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IUser, User>();
            services.AddScoped<ILibrary, Library>();
            services.AddScoped<SessionMgt, SessionMgt>();
            services.AddScoped<IBundleConfiguration, BundleConfiguration>();
            services.AddScoped<IReaderBookService, ReaderBookService>();
            services.AddScoped<IReaderBooks, ReaderBooks>();
            services.AddScoped<IEbookReader, EbookReader>();
            services.AddScoped<ReadEdgeUserLoginInfo, ReadEdgeUserLoginInfo>();


            ///===============

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

             app.UseExceptionHandler("/Home/Error");
             app.UseGleamTech();
           // DocumentUltimateConfiguration.Current.LicenseKey = "YGHMATRXDZ-HAUWP8LXUZ-BUAB7S1KTG-5JCZ1CUDRH-APE27QXK2F-U3HUSF4MCX-NSFZ64XN7H-F";
            DocumentUltimateConfiguration.Current.LicenseKey = "QUHPE7A8M6-2LXQ79ASGX-88CBL3DQ19-UKYXWZ1CUD-RHAPC27QXK-2FU3HUSF4M-CXNSFZ64XN-7HF";
            //The default CacheLocation value is "~/App_Data/DocumentCache"
            //Both virtual and physical paths are allowed (or a Location instance for one of the supported 
            //file systems like Amazon S3 and Azure Blob).
            DocumentUltimateWebConfiguration.Current.CacheLocation = "~/App_Data/DocumentCache";
            app.UseStaticFiles();

            app.UseSession();
            app.UseCookiePolicy();
            //app.Use(async (context, next) =>
            //{
            //    var a = _httpContextAccessor;
            //    int? login = 0;
            //    if(_httpContextAccessor!=null)
            //    //var c = _httpContextAccessor.HttpContext.Session;
            //    //var cultureQuery = context.Request.Query["culture"];
            //    login = _httpContextAccessor.HttpContext.Session.GetInt32("login");
               
            //    if (login != 1)
            //    {
            //        context.Response.Redirect("/Account/Login");
            //        return;
            //    }

            //    // Call the next delegate/middleware in the pipeline
            //    await next();
            //});
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSignalR(endpoints =>
            {
                endpoints.MapHub<BroadcastHub>("/BroadcastHub");
            });

        }
    }
}
