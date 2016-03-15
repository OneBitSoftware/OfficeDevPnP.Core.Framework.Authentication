namespace OfficeDevPnP.Core.Framework.Authentication
{
    public interface ISharePointConfiguration
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IssuerId { get; set; }
        string HostedAppHostNameOverride { get; set; }
        string HostedAppHostName { get; set; }
        string SecondaryClientSecret { get; set; }
        string Realm { get; set; }
        string ClientSigningCertificatePath { get; set; }
        string ClientSigningCertificatePassword { get; set; }
    }
}
