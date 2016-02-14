using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public static class OAuth2MessageFactory
    {
        public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAssertion(SecurityToken token, string resource)
        {
            if (token == null) { throw new ArgumentException("token"); }
            //SecurityTokenHandlerCollection securityTokenHandlerCollection = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
            //securityTokenHandlerCollection.Add(new JwtSecurityTokenHandler());
            //return OAuth2MessageFactory.CreateAccessTokenRequestWithAssertion(token, securityTokenHandlerCollection, resource);
            return OAuth2MessageFactory.CreateAccessTokenRequestWithAssertion(token as JwtSecurityToken, new JwtSecurityTokenHandler(), resource);
        }

        //public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAssertion(SecurityToken token, SecurityTokenHandlerCollection securityTokenHandlers, string resource)
        //{
        //    if (token == null) { throw new ArgumentException("token"); }

        //    if (token is JwtSecurityToken)
        //    {
        //        return OAuth2MessageFactory.CreateAccessTokenRequestWithAssertion((JwtSecurityToken)token, securityTokenHandlers, resource);
        //    }
        //    //if (token is GenericXmlSecurityToken)
        //    //{
        //    //    return OAuth2MessageFactory.CreateAccessTokenRequestWithAssertion((GenericXmlSecurityToken)token, resource);
        //    //}
        //    //if (!(token is SamlSecurityToken) && !(token is Saml2SecurityToken))
        //    //{
        //    //    throw new ArgumentException("Unsupported SecurityToken");
        //    //}
        //    return OAuth2MessageFactory.CreateAccessTokenRequestWithAssertionForSamlSecurityTokens(token, securityTokenHandlers, resource);
        //}

//        private static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAssertion(GenericXmlSecurityToken token, string resource)
//        {
//            string str;
//            if (token == null) { throw new ArgumentException("token"); }

//            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest();
//            JwtSecurityTokenHandler jsonWebSecurityTokenHandler = new JwtSecurityTokenHandler();
////            XmlReader xmlNodeReader = new XmlNodeReader(token.TokenXml);
//            string jsonTokenString = jsonWebSecurityTokenHandler.GetJsonTokenString(xmlNodeReader, out str);
//            oAuth2AccessTokenRequest.GrantType = OAuth2MessageFactory.GetTokenType(token);
//            oAuth2AccessTokenRequest.Assertion = jsonTokenString;
//            oAuth2AccessTokenRequest.Resource = resource;
//            return oAuth2AccessTokenRequest;
//        }

        //private static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAssertion(JwtSecurityToken token, SecurityTokenHandlerCollection securityTokenHandlers, string resource)
        private static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAssertion(JwtSecurityToken token, JwtSecurityTokenHandler handler, string resource)
        {
            if (token == null) { throw new ArgumentException("token"); }
            if (handler== null) { throw new ArgumentException("securityTokenHandlers"); }

            JwtSecurityTokenHandler item = handler;//securityTokenHandlers[typeof(JwtSecurityToken)] as JwtSecurityTokenHandler;
            if (item == null)
            {
                throw new ArgumentException("The input security token handlers collection does not contain a handler for JWT tokens.", "securityTokenHandlers");
            }
            string str = item.WriteToken(token);
            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest()
            {
                GrantType = "http://oauth.net/grant_type/jwt/1.0/bearer",
                Assertion = str,
                Resource = resource
            };
            return oAuth2AccessTokenRequest;
        }

        //private static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAssertionForSamlSecurityTokens(SecurityToken token, SecurityTokenHandlerCollection securityTokenHandlers, string resource)
        //{
        //    if (securityTokenHandlers == null) { throw new ArgumentException("securityTokenHandlers"); }

        //    OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest();
        //    if (!(token is SamlSecurityToken))
        //    {
        //        oAuth2AccessTokenRequest.GrantType = "urn:oasis:names:tc:SAML:2.0:assertion";
        //    }
        //    else
        //    {
        //        oAuth2AccessTokenRequest.GrantType = "urn:oasis:names:tc:SAML:1.0:assertion";
        //    }
        //    XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
        //    StringBuilder stringBuilder = new StringBuilder();
        //    xmlWriterSetting.OmitXmlDeclaration = true;
        //    using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSetting))
        //    {
        //        securityTokenHandlers.WriteToken(xmlWriter, token);
        //        oAuth2AccessTokenRequest.Assertion = stringBuilder.ToString();
        //    }
        //    oAuth2AccessTokenRequest.Resource = resource;
        //    return oAuth2AccessTokenRequest;
        //}

        public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAuthorizationCode(string clientId, string clientSecret, string authorizationCode, Uri redirectUri, string resource)
        {
            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest()
            {
                GrantType = "authorization_code",
                ClientId = clientId,
                ClientSecret = clientSecret,
                Code = authorizationCode
            };
            if (redirectUri != null)
            {
                oAuth2AccessTokenRequest.RedirectUri = redirectUri.AbsoluteUri;
            }
            oAuth2AccessTokenRequest.Resource = resource;
            return oAuth2AccessTokenRequest;
        }

        public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithAuthorizationCode(string clientId, string clientSecret, string authorizationCode, string resource)
        {
            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest()
            {
                GrantType = "authorization_code",
                ClientId = clientId,
                ClientSecret = clientSecret,
                Code = authorizationCode,
                Resource = resource
            };
            return oAuth2AccessTokenRequest;
        }

        public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithClientCredentials(string clientId, string clientSecret, string scope)
        {
            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest()
            {
                GrantType = "client_credentials",
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            };
            return oAuth2AccessTokenRequest;
        }

        public static OAuth2AccessTokenRequest CreateAccessTokenRequestWithRefreshToken(string clientId, string clientSecret, string refreshToken, string resource)
        {
            OAuth2AccessTokenRequest oAuth2AccessTokenRequest = new OAuth2AccessTokenRequest()
            {
                GrantType = "refresh_token",
                ClientId = clientId,
                ClientSecret = clientSecret,
                RefreshToken = refreshToken,
                Resource = resource
            };
            return oAuth2AccessTokenRequest;
        }

        public static OAuth2Message CreateFromEncodedResponse(StreamReader reader)
        {
            return OAuth2MessageFactory.CreateFromEncodedResponse(reader.ReadToEnd());
        }

        public static OAuth2Message CreateFromEncodedResponse(string responseString)
        {
            if (responseString.StartsWith("{\"error"))
            {
                throw new Exception("Response included errors.");
                //return OAuth2ErrorResponse.CreateFromEncodedResponse(responseString);
            }
            return OAuth2AccessTokenResponse.Read(responseString);
        }

        //private static string GetTokenType(GenericXmlSecurityToken token)
        //{
        //    string attribute;
        //    using (XmlReader xmlNodeReader = new XmlNodeReader(token.TokenXml))
        //    {
        //        xmlNodeReader.MoveToContent();
        //        if (!xmlNodeReader.IsStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
        //        {
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            attribute = xmlNodeReader.GetAttribute("ValueType", null);
        //        }
        //    }
        //    return attribute;
        //}
    }
}
