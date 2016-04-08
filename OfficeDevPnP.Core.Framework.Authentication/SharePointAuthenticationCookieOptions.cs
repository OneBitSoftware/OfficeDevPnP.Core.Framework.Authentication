namespace OfficeDevPnP.Core.Framework.Authentication
{
    using Microsoft.AspNet.Authentication.Cookies;

    public class SharePointAuthenticationCookieOptions
    {
        /// <summary>
        /// Gets or sets the scheme used to identify application authentication cookies.
        /// </summary>
        /// <value>The scheme used to identify application authentication cookies.</value>
        public string ApplicationCookieAuthenticationScheme { get; set; } = typeof(SharePointAuthenticationCookieOptions).Namespace + ".Application";

        public CookieAuthenticationOptions ApplicationCookie { get; set; }

        /// <summary>
        /// Default constructor for SharePointContext Cookie Options. Internally instantiates CookieAuthenticationOptions with default values.
        /// </summary>
        public SharePointAuthenticationCookieOptions()
        {
            // Configure all of the cookie middlewares
            ApplicationCookie = new CookieAuthenticationOptions
            {
                AuthenticationScheme = ApplicationCookieAuthenticationScheme,
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ExpireTimeSpan = System.TimeSpan.FromDays(14)
            };
        }

        /// <summary>
        /// Constructor accepting CookieAuthenticationOptions
        /// </summary>
        /// <param name="cookieOptions"></param>
        public SharePointAuthenticationCookieOptions(CookieAuthenticationOptions cookieOptions)
        {
            // Configure all of the cookie middlewares
            ApplicationCookie = cookieOptions;
        }
    }
}
