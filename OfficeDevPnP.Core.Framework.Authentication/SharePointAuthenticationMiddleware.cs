using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using System;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    /// <summary>
    /// An ASP.NET Core middleware for authenticating users using SharePoint.
    /// </summary>
    public class SharePointAuthenticationMiddleware :
        AuthenticationMiddleware<SharePointAuthenticationOptions>
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new <see cref="SharePointAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="options">Configuration options for the middleware.</param>
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

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler{T}"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler{T}"/> configured with the <see cref="SharePointAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<SharePointAuthenticationOptions> CreateHandler()
        {
            return new SharePointAuthenticationHandler();
        }
    }
}
