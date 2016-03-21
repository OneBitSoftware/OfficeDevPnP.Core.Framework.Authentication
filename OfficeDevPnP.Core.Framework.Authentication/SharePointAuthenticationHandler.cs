using Microsoft.AspNet.Authentication;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using System.Linq;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationHandler : AuthenticationHandler<SharePointAuthenticationOptions>
    {
        /// <summary>
        /// RedirectionStatus would decide if we contunue to the next middleware in the pipe.
        /// </summary>
        private RedirectionStatus _redirectionStatus;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Uri redirectUrl;
            var defaultScheme = SharePointAuthenticationDefaults.AuthenticationScheme;
            var cookieScheme = new SharePointContextCookieOptions().ApplicationCookie.AuthenticationScheme;
            AuthenticateResult result = AuthenticateResult.Failed("Could not get the RedirectionStatus");

            // Sets up the SharePoint configuration based on the middleware options.
            SharePointContextProvider.GetInstance(SharePointConfiguration.GetFromSharePointAuthenticationOptions(Options));
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    _redirectionStatus = RedirectionStatus.Ok;
                    
                    // Checks if we already have authenticated principal.
                    ClaimsPrincipal principal;
                    if (Context.User.Identities.Any(identity => identity.IsAuthenticated))
                    {
                        principal = Context.User;
                    }
                    else
                    {
                        // The cookie authentication is listening for.
                        var identity = new ClaimsIdentity(defaultScheme);

                        // Adds empty claims to the Identity object.
                        var newClaims = new[]
                        {
                            new Claim(ClaimTypes.AuthenticationMethod, defaultScheme),
                        };
                        identity.AddClaims(newClaims);

                        // Creates the authentication ticket.
                        principal = new ClaimsPrincipal(identity);

                        principal.AddIdentity(identity);
                        // Handles the sign in method of the auth middleware.
                        
                        await Context.Authentication.SignInAsync(cookieScheme, principal);
                        //, new AuthenticationProperties()
                        //  {
                        //      ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                        //      IsPersistent = false,
                        //      AllowRefresh = false
                        //  }
                    }
                    var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), cookieScheme);
                    result = AuthenticateResult.Success(ticket);
                    break;
                case RedirectionStatus.ShouldRedirect:
                    _redirectionStatus = RedirectionStatus.ShouldRedirect;

                    Response.StatusCode = 301;
                    result = AuthenticateResult.Failed("ShouldRedirect");
                    Context.Response.Redirect(redirectUrl.AbsoluteUri);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    _redirectionStatus = RedirectionStatus.CanNotRedirect;

                    Response.StatusCode = 401;
                    result = AuthenticateResult.Failed("CanNotRedirect");
                    break;
            }
            return result;
        }
        
        protected override async Task HandleSignInAsync(SignInContext context)
        {
            await base.HandleSignInAsync(context);
            SignInAccepted = true;
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            return await base.HandleUnauthorizedAsync(context);
        }

        public override async Task<bool> HandleRequestAsync()
        {
            if (_redirectionStatus == RedirectionStatus.ShouldRedirect)
            {
                // Stops the execution of next middlewares since redirect to SharePoint is required.
                return await Task.FromResult(true);
            }
            return await base.HandleRequestAsync();
        }


        protected override async Task HandleSignOutAsync(SignOutContext context)
        {
            await Context.Authentication.SignOutAsync(SharePointAuthenticationDefaults.AuthenticationScheme);
            await base.HandleSignOutAsync(context);
        }
    }
}
