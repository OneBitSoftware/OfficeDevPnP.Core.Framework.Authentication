using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Server.Kestrel.Https;

namespace AspNet5.Mvc6.StarterWeb.App_Start
{
    public class WebServerConfig
    {
        public static void ConfigureSSL(IApplicationBuilder app, string certPath, string password)
        {
            if (string.IsNullOrEmpty(certPath))
            {
                throw new ArgumentException("Missing X509Certificate2. Cannot start on SSL.");
            }
            app.UseKestrelHttps(new X509Certificate2(certPath, password));
        }
    }
}
