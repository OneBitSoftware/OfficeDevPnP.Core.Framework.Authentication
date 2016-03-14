using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Http.Features;

namespace OfficeDevPnP.Core.Framework.Authentication  //TODO; should we use Microsoft.Extensions.DependencyInjection as it is done on the rest of the Microsoft middleware
{
    /// <summary>
    /// Extension methods for setting up authentication services in an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
    /// </summary>
    public static class SharePointAuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// Adds authentication services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// /// <param name="configuration">The <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" /> to add services to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSharePointAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            //services.AddTransient<ISession>();
            //services.AddTransient<IConfigurationSection>();
            //OptionsServiceCollectionExtensions.Configure<IConfiguration>(services, configuration);
            return services;
        }

        /// <summary>
        /// Adds authentication services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="T:Microsoft.AspNet.Authentication.SharedAuthenticationOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSharePointAuthentication(this IServiceCollection services, IConfiguration configuration, Action<SharePointAuthenticationOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            if (configureOptions == null)
            {
                throw new ArgumentNullException("configureOptions");
            }
            OptionsServiceCollectionExtensions.Configure<SharePointAuthenticationOptions>(services, configureOptions);
            return services.AddSharePointAuthentication(configuration);
        }
    }
}