using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointHighTrustContextProvider : SharePointContextProvider
    {
        public SharePointHighTrustContextProvider()
        {
            throw new NotImplementedException();
        }

        protected override SharePointContext CreateSharePointContext(Uri spHostUrl, Uri spAppWebUrl, string spLanguage, string spClientTag, string spProductNumber, HttpRequest httpRequest)
        {
            throw new NotImplementedException();
        }

        protected override SharePointContext LoadSharePointContext(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        protected override void SaveSharePointContext(SharePointContext spContext, HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        protected override bool ValidateSharePointContext(SharePointContext spContext, HttpContext httpContext)
        {
            throw new NotImplementedException();
        }
    }
}
