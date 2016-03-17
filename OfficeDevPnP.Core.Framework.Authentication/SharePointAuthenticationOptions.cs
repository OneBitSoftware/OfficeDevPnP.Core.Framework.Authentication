using Microsoft.AspNet.Authentication;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationOptions : AuthenticationOptions
    {
        public SharePointAuthenticationOptions() : base()
        {
            //Set automatic challenge to default
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
            AuthenticationScheme = SharePointAuthenticationDefaults.AuthenticationScheme;
        }

        /// <summary>
        /// Gets or sets if HTTPS is required for the metadata address or authority.
        /// The default is true. This should be disabled only in development environments.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IssuerId { get; set; }
        public string HostedAppHostNameOverride { get; set; }
        public string HostedAppHostName { get; set; }
        public string SecondaryClientSecret { get; set; }
        public string Realm { get; set; }
        public string ClientSigningCertificatePath { get; set; }
        public string ClientSigningCertificatePassword { get; set; }
    }
}
