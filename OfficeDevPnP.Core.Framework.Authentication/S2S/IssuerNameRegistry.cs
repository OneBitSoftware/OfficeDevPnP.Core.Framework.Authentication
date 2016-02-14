using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public abstract class IssuerNameRegistry
    {
        protected IssuerNameRegistry()
        {
        }

        public abstract string GetIssuerName(SecurityToken securityToken);

        public virtual string GetIssuerName(SecurityToken securityToken, string requestedIssuerName)
        {
            return this.GetIssuerName(securityToken);
        }

        public virtual string GetWindowsIssuerName()
        {
            return "LOCAL AUTHORITY";
        }
    }
}
