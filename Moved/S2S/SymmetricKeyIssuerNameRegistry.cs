using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public class SymmetricKeyIssuerNameRegistry : IssuerNameRegistry
    {
        private Dictionary<string, string> _issuerList = new Dictionary<string, string>();

        public SymmetricKeyIssuerNameRegistry()
        {
        }

        public void AddTrustedIssuer(byte[] symmetricKey, string issuerName)
        {
            if (symmetricKey == null) throw new ArgumentException("securityToken");
            if (issuerName == null) throw new ArgumentException("issuerName");

            this._issuerList.Add(Convert.ToBase64String(symmetricKey), issuerName);
        }

        public override string GetIssuerName(SecurityToken securityToken)
        {
            if (securityToken == null) throw new ArgumentException("securityToken");

            string str = null;
            BinarySecretSecurityToken binarySecretSecurityToken = securityToken as BinarySecretSecurityToken;
            if (binarySecretSecurityToken != null)
            {
                this._issuerList.TryGetValue(Convert.ToBase64String(binarySecretSecurityToken.GetKeyBytes()), out str);
            }
            return str;
        }
    }
}
