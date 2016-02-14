using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointContextToken : JwtSecurityToken
    {
        public static SharePointContextToken Create(JwtSecurityToken contextToken)
        {
            //BUG: not supporting multiple audiences, but seems OK for SharePoint
            string audience = contextToken.Audiences.FirstOrDefault();
            return new SharePointContextToken(
                contextToken.Issuer,
                audience,
                contextToken.Claims,
                contextToken.ValidFrom,
                contextToken.ValidTo,
                contextToken.SigningCredentials);
        }

        public SharePointContextToken(string issuer, string audience, IEnumerable<Claim> claims,
            DateTime validFrom, DateTime validTo, SigningCredentials signingCredentials)
            : base(issuer, audience, claims, validFrom, validTo, signingCredentials)
        {
        }

        public SharePointContextToken(string jwtEncodedString) : base (jwtEncodedString)
        {
        }

        public string NameId
        {
            get
            {
                return GetClaimValue(this, "nameid");
            }
        }

        /// <summary>
        /// The principal name portion of the context token's "appctxsender" claim
        /// </summary>
        public string TargetPrincipalName
        {
            get
            {
                string appctxsender = GetClaimValue(this, "appctxsender");

                if (appctxsender == null)
                {
                    return null;
                }

                return appctxsender.Split('@')[0];
            }
        }

        /// <summary>
        /// The context token's "refreshtoken" claim
        /// </summary>
        public string RefreshToken
        {
            get
            {
                return GetClaimValue(this, "refreshtoken");
            }
        }

        /// <summary>
        /// The context token's "CacheKey" claim
        /// </summary>
        public string CacheKey
        {
            get
            {
                string appctx = GetClaimValue(this, "appctx");
                if (appctx == null)
                {
                    return null;
                }

                ClientContext ctx = new ClientContext("http://tempuri.org");
                Dictionary<string, object> dict = (Dictionary<string, object>)ctx.ParseObjectFromJsonString(appctx);
                string cacheKey = (string)dict["CacheKey"];

                return cacheKey;
            }
        }

        /// <summary>
        /// The context token's "SecurityTokenServiceUri" claim
        /// </summary>
        public string SecurityTokenServiceUri
        {
            get
            {
                string appctx = GetClaimValue(this, "appctx");
                if (appctx == null)
                {
                    return null;
                }

                ClientContext ctx = new ClientContext("http://tempuri.org");
                Dictionary<string, object> dict = (Dictionary<string, object>)ctx.ParseObjectFromJsonString(appctx);
                string securityTokenServiceUri = (string)dict["SecurityTokenServiceUri"];

                return securityTokenServiceUri;
            }
        }

        /// <summary>
        /// The realm portion of the context token's "audience" claim
        /// </summary>
        public string Realm
        {
            get
            {
                //BUG: not supporting multiple audiences, but seems OK for SharePoint
                string aud = this.Audiences.FirstOrDefault();
                if (aud == null)
                {
                    return null;
                }

                string tokenRealm = aud.Substring(aud.IndexOf('@') + 1);

                return tokenRealm;
            }
        }

        private static string GetClaimValue(JwtSecurityToken token, string claimType)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            foreach (Claim claim in token.Claims)
            {
                if (StringComparer.Ordinal.Equals(claim.Type, claimType))
                {
                    return claim.Value;
                }
            }

            return null;
        }

    }

}
