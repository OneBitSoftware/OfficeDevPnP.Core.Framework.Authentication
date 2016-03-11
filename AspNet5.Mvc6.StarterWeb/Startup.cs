using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Server.Kestrel.Https;
using Microsoft.AspNet.Session;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Caching.Memory;
using OfficeDevPnP.Core.Framework.Authentication;

namespace AspNet5.Mvc6.StarterWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication();
            services.AddCaching();

            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(10);
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var testCertPath = Path.Combine(env.WebRootPath, @"../../cert/office365flask.pfx");
            if (string.IsNullOrEmpty(testCertPath))
            {
                throw new ArgumentException("Missing X509Certificate2. Cannot start on SSL.");
            }
            app.UseKestrelHttps(new X509Certificate2(testCertPath, "pass@word1"));

            //app.UseIISPlatformHandler();

            app.UseSession(); //TODO: should we move this as SharePointAuthenticationOption
            app.UseStaticFiles();

            //Add SharePoint authentication capabilities
            app.UseSharePointAuthentication(new SharePointAuthenticationOptions()
                    {
                        RequireHttpsMetadata = true
                    }
                );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
