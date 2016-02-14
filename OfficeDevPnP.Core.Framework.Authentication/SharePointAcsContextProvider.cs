using Microsoft.AspNet.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    /// <summary>
    /// Default provider for SharePointAcsContext.
    /// </summary>
    public class SharePointAcsContextProvider : SharePointContextProvider
    {
        private const string SPContextKey = "SPContext";
        private const string SPCacheKeyKey = "SPCacheKey";

        protected override SharePointContext CreateSharePointContext
            (Uri spHostUrl, Uri spAppWebUrl, string spLanguage, string spClientTag,
            string spProductNumber, HttpRequest httpRequest)
        {
            string contextTokenString = TokenHelper.GetContextTokenFromRequest(httpRequest);
            if (string.IsNullOrEmpty(contextTokenString))
            {
                return null;
            }

            SharePointContextToken contextToken = null;
            try
            {
                contextToken = TokenHelper.ReadAndValidateContextToken(contextTokenString,
                    //httpRequest.Url.Authority);
                    httpRequest.PathBase);
            }
            catch //TODO type WebException?
            {
                return null;
            }

            return new SharePointAcsContext(spHostUrl, spAppWebUrl, spLanguage, spClientTag, spProductNumber, contextTokenString, contextToken);
        }

        protected override bool ValidateSharePointContext(SharePointContext spContext,
            HttpContext httpContext)
        {
            SharePointAcsContext spAcsContext = spContext as SharePointAcsContext;

            if (spAcsContext != null)
            {
                Uri spHostUrl = SharePointContext.GetSPHostUrl(httpContext.Request);
                string contextToken = TokenHelper.GetContextTokenFromRequest(httpContext.Request);
                //Cookie spCacheKeyCookie = httpContext.Request.Cookies[SPCacheKeyKey].ToString();
                string spCacheKey = httpContext.Request.Cookies[SPCacheKeyKey].ToString(); //spCacheKeyCookie != null ? spCacheKeyCookie.Value : null;

                return spHostUrl == spAcsContext.SPHostUrl &&
                       !string.IsNullOrEmpty(spAcsContext.CacheKey) &&
                       spCacheKey == spAcsContext.CacheKey &&
                       !string.IsNullOrEmpty(spAcsContext.ContextToken) &&
                       (string.IsNullOrEmpty(contextToken) || contextToken == spAcsContext.ContextToken);
            }

            return false;
        }

        protected override SharePointContext LoadSharePointContext(HttpContext httpContext)
        {
            var sessionString = httpContext.Session.GetString(SPContextKey);
            return JsonConvert.DeserializeObject(sessionString) as SharePointAcsContext;
        }

        protected override void SaveSharePointContext(SharePointContext spContext, HttpContext httpContext)
        {
            SharePointAcsContext spAcsContext = spContext as SharePointAcsContext;

            if (spAcsContext != null)
            {
                //Cookie spCacheKeyCookie = new Cookie(SPCacheKeyKey, spAcsContext.CacheKey)
                //{
                //    Secure = true,
                //    HttpOnly = true
                //};

                httpContext.Response.Cookies.Append(SPCacheKeyKey, spAcsContext.CacheKey);
            }

            var serializedContext = JsonConvert.SerializeObject(spAcsContext);
            httpContext.Session.SetString(SPContextKey, serializedContext);
        }
    }
}
