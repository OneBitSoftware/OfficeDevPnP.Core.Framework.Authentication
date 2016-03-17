using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IssuerId { get; set; }
        public string HostedAppHostNameOverride { get; set; }
        public string HostedAppHostName { get; set; }
        public string SecondaryClientSecret { get; set; }
        public string Realm { get; set; }
        public string ClientSigningCertificatePath { get; set; }
        public string ClientSigningCertificatePassword { get; set; }

        public static SharePointConfiguration GetFromSharePointAuthenticationOptions(SharePointAuthenticationOptions options)
        {
            return new SharePointConfiguration()
            {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                IssuerId = options.IssuerId,
                HostedAppHostNameOverride = options.HostedAppHostNameOverride,
                HostedAppHostName = options.HostedAppHostName,
                SecondaryClientSecret = options.SecondaryClientSecret,
                Realm = options.Realm,
                ClientSigningCertificatePath = options.ClientSigningCertificatePath,
                ClientSigningCertificatePassword = options.ClientSigningCertificatePassword
            };
        }

        public static SharePointConfiguration GetFromIOptions(IOptions<SharePointConfiguration> options)
        {
            return new SharePointConfiguration()
            {
                ClientId = options.Value.ClientId,
                ClientSecret = options.Value.ClientSecret,
                IssuerId = options.Value.IssuerId,
                HostedAppHostNameOverride = options.Value.HostedAppHostNameOverride,
                HostedAppHostName = options.Value.HostedAppHostName,
                SecondaryClientSecret = options.Value.SecondaryClientSecret,
                Realm = options.Value.Realm,
                ClientSigningCertificatePath = options.Value.ClientSigningCertificatePath,
                ClientSigningCertificatePassword = options.Value.ClientSigningCertificatePassword
            };
        }

        public static SharePointConfiguration GetFromIConfiguration(IConfiguration configuration)
        {
            return new SharePointConfiguration()
            {
                ClientId = configuration["ClientId"],
                ClientSecret = configuration["ClientSecret"],
                IssuerId = configuration["IssuerId"],
                HostedAppHostNameOverride = configuration["HostedAppHostNameOverride"],
                HostedAppHostName = configuration["HostedAppHostName"],
                SecondaryClientSecret = configuration["SecondaryClientSecret"],
                Realm = configuration["Realm"],
                ClientSigningCertificatePath = configuration["ClientSigningCertificatePath"],
                ClientSigningCertificatePassword = configuration["ClientSigningCertificatePassword"]
            };
        }
    }
}
