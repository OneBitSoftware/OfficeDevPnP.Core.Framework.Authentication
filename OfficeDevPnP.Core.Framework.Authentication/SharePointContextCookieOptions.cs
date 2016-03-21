using Microsoft.AspNet.Authentication.Cookies;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointContextCookieOptions
    {
        /// <summary>
        /// Gets or sets the scheme used to identify application authentication cookies.
        /// </summary>
        /// <value>The scheme used to identify application authentication cookies.</value>
        public string ApplicationCookieAuthenticationScheme { get; set; } = typeof(SharePointContextCookieOptions).Namespace + ".Application";

        public CookieAuthenticationOptions ApplicationCookie { get; set; }

        /// <summary>
        /// Default constructor for SharePointContext Cookie Options. Internally instantiates CookieAuthenticationOptions with default values.
        /// </summary>
        public SharePointContextCookieOptions()
        {
            // Configure all of the cookie middlewares
            ApplicationCookie = new CookieAuthenticationOptions
            {
                AuthenticationScheme = ApplicationCookieAuthenticationScheme,
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            };
        }

        /// <summary>
        /// Constructor accepting CookieAuthenticationOptions
        /// </summary>
        /// <param name="cookieOptions"></param>
        public SharePointContextCookieOptions(CookieAuthenticationOptions cookieOptions)
        {
            // Configure all of the cookie middlewares
            ApplicationCookie = cookieOptions;
        }
    }
}
