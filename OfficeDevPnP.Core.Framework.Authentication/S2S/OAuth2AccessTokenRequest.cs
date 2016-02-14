﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public class OAuth2AccessTokenRequest : OAuth2Message
    {
        public static StringCollection TokenResponseParameters;

        public string AppContext
        {
            get
            {
                return base["AppContext"];
            }
            set
            {
                base["AppContext"] = value;
            }
        }

        public string Assertion
        {
            get
            {
                return base["assertion"];
            }
            set
            {
                base["assertion"] = value;
            }
        }

        public string ClientId
        {
            get
            {
                return base["client_id"];
            }
            set
            {
                base["client_id"] = value;
            }
        }

        public string ClientSecret
        {
            get
            {
                return base["client_secret"];
            }
            set
            {
                base["client_secret"] = value;
            }
        }

        public string Code
        {
            get
            {
                return base["code"];
            }
            set
            {
                base["code"] = value;
            }
        }

        public string GrantType
        {
            get
            {
                return base["grant_type"];
            }
            set
            {
                base["grant_type"] = value;
            }
        }

        public string Password
        {
            get
            {
                return base.Message["password"];
            }
            set
            {
                base.Message["password"] = value;
            }
        }

        public string RedirectUri
        {
            get
            {
                return base["redirect_uri"];
            }
            set
            {
                base["redirect_uri"] = value;
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

        public string Resource
        {
            get
            {
                return base.Message["resource"];
            }
            set
            {
                base.Message["resource"] = value;
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

        static OAuth2AccessTokenRequest()
        {
            OAuth2AccessTokenRequest.TokenResponseParameters = OAuth2AccessTokenRequest.GetTokenResponseParameters();
        }

        public OAuth2AccessTokenRequest()
        {
        }

        private static StringCollection GetTokenResponseParameters()
        {
            StringCollection stringCollections = new StringCollection();
            stringCollections.Add("access_token");
            stringCollections.Add("expires_in");
            return stringCollections;
        }

        public static OAuth2AccessTokenRequest Read(StreamReader reader)
        {
            string end = null;
            try
            {
                end = reader.ReadToEnd();
            }
            catch (IOException decoderFallbackException)
            {
                throw new InvalidDataException("Request encoding is not ASCII", decoderFallbackException);
            }
            return OAuth2AccessTokenRequest.Read(end);
        }

        public static OAuth2AccessTokenRequest Read(string requestString)
        {
            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest();
            try
            {
                oAuth2AccessTokenRequest.Decode(requestString);
            }
            catch (Exception invalidRequestException)
            {
                    //if (string.IsNullOrEmpty(nameValueCollection["client_id"]) && string.IsNullOrEmpty(nameValueCollection["assertion"]))
                    throw new InvalidDataException("The request body must contain a client_id or assertion parameter.");
            }
            foreach (string key in oAuth2AccessTokenRequest.Keys)
            {
                if (!OAuth2AccessTokenRequest.TokenResponseParameters.Contains(key))
                {
                    continue;
                }
                throw new InvalidDataException();
            }
            return oAuth2AccessTokenRequest;
        }

        public void SetCustomProperty(string propertyName, string propertyValue)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(propertyValue))
            {
                throw new ArgumentException();
            }

            base[propertyName] = propertyValue;
        }

        public virtual void Write(StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            writer.Write(base.Encode());
        }
    }
}
