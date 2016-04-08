using System;
using System.IO;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.AddCaching();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(3600);
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //required to store SP Cache Key session data
            app.UseSession();

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

            //Configuer SSL, only needed due to Kestrel
            //TODO: inject through IOptions and appconfig.json
            // Should Not be a concern of the application
            WebServerConfig.ConfigureSSL(app, Path.Combine(env.WebRootPath, @"../../cert/office365flask.pfx"), "pass@word1");

            app.UseStaticFiles();

            //Required to do client session management, but uses the schema of our middleware
            app.UseCookieAuthentication(new SharePointAuthenticationCookieOptions().ApplicationCookie);

            //Add SharePoint authentication capabilities
            app.UseSharePointAuthentication(new SharePointAuthenticationOptions()
                {
                    RequireHttpsMetadata = true,
                    ClientId = Configuration["SharePointAuthentication:ClientId"],
                    ClientSecret = Configuration["SharePointAuthentication:ClientSecret"]
                }
            );

            //set up MVC routes
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
