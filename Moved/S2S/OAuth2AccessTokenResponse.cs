using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public class OAuth2AccessTokenResponse : OAuth2Message
    {
        public string AccessToken
        {
            get
            {
                return base.Message["access_token"];
            }
            set
            {
                base.Message["access_token"] = value;
            }
        }

        public virtual string ExpiresIn
        {
            get
            {
                return base.Message["expires_in"];
            }
            set
            {
                base.Message["expires_in"] = value;
            }
        }

        public DateTime ExpiresOn
        {
            get
            {
                return this.GetDateTimeParameter("expires_on");
            }
            set
            {
                this.SetDateTimeParameter("expires_on", value);
            }
        }

        public DateTime NotBefore
        {
            get
            {
                return this.GetDateTimeParameter("not_before");
            }
            set
            {
                this.SetDateTimeParameter("not_before", value);
            }
        }

        public string RefreshToken
        {
            get
            {
                return base.Message["refresh_token"];
            }
            set
            {
                base.Message["refresh_token"] = value;
            }
        }

        public string Scope
        {
            get
            {
                return base.Message["scope"];
            }
            set
            {
                base.Message["scope"] = value;
            }
        }

        public string TokenType
        {
            get
            {
                return base.Message["token_type"];
            }
            set
            {
                base.Message["token_type"] = value;
            }
        }

        public OAuth2AccessTokenResponse()
        {
        }

        private DateTime GetDateTimeParameter(string parameterName)
        {
            var epochTime = Convert.ToInt32(base.Message[parameterName]);
            return (EpochTime.DateTime(epochTime));
        }

        public static OAuth2AccessTokenResponse Read(string responseString)
        {
            OAuth2AccessTokenResponse oAuth2AccessTokenResponse = new OAuth2AccessTokenResponse();
            oAuth2AccessTokenResponse.DecodeFromJson(responseString);
            return oAuth2AccessTokenResponse;
        }

        private void SetDateTimeParameter(string parameterName, DateTime value)
        {
            Dictionary<string, string> message = base.Message;
            long secondsSinceUnixEpoch = (EpochTime.GetIntDate(value));
            message[parameterName] = secondsSinceUnixEpoch.ToString();
        }

        public override string ToString()
        {
            return base.EncodeToJson();
        }
    }
}
