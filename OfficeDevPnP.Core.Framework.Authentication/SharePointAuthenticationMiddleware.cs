using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using System;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationMiddleware :
        AuthenticationMiddleware<SharePointAuthenticationOptions>
    {
        private readonly RequestDelegate _next;

        public SharePointAuthenticationMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder,
            SharePointAuthenticationOptions options)
            : base(next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _next = next;
        }

        protected override AuthenticationHandler<SharePointAuthenticationOptions> CreateHandler()
        {
            return new SharePointAuthenticationHandler();
        }
    }
}
