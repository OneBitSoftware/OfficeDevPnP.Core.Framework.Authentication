using Microsoft.AspNet.Authentication;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Sets default options.
        /// </summary>
        public SharePointAuthenticationOptions()
        {
            // Sets automatic challenge to default.
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
            AuthenticationScheme = SharePointAuthenticationDefaults.AuthenticationScheme;
        }

        /// <summary>
        /// Gets or sets if HTTPS is required for the metadata address or authority.
        /// The default is true. This should be disabled only in development environments.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;

        /// <summary>
        /// Gets or sets ClientId.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets ClientSecret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets IssuerId.
        /// </summary>
        public string IssuerId { get; set; }

        /// <summary>
        /// Gets or sets HostedAppHostNameOverride.
        /// </summary>
        public string HostedAppHostNameOverride { get; set; }

        /// <summary>
        /// Gets or sets HostedAppHostName.
        /// </summary>
        public string HostedAppHostName { get; set; }

        /// <summary>
        /// Gets or sets SecondaryClientSecret.
        /// </summary>
        public string SecondaryClientSecret { get; set; }

        /// <summary>
        /// Gets or sets Realm.
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets ClientSigningCertificatePath.
        /// </summary>
        public string ClientSigningCertificatePath { get; set; }

        /// <summary>
        /// Gets or sets ClientSigningCertificatePassword.
        /// </summary>
        public string ClientSigningCertificatePassword { get; set; }
    }
}
