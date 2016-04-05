﻿using Microsoft.AspNet.Authentication;
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

            //Set the default error message when no SP Auth is attempted
            AuthenticateResult result = AuthenticateResult.Failed("Could not handle SharePoint authentication.");

            var authenticationProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                IsPersistent = false,
                AllowRefresh = false
            };

            // Sets up the SharePoint configuration based on the middleware options.
            SharePointContextProvider.GetInstance(SharePointConfiguration.GetFromSharePointAuthenticationOptions(Options));

            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    _redirectionStatus = RedirectionStatus.Ok;

                    // Gets the current SharePoint context
                    var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);

                    // Gets the SharePoint context CacheKey. The CacheKey would be assigned as issuer for new claim.
                    // It is also used to validate identity that is authenticated.
                    //TODO: would not work with HighTrust at the moment
                    var claimIssuer = ((SharePointAcsContext)spContext).CacheKey; 

                    // Checks if we already have authenticated principal.
                    ClaimsPrincipal principal;
                    if (Context.User.Identities.Any(identity => identity.IsAuthenticated && identity.HasClaim(x => x.Issuer == claimIssuer)))
                    {
                        principal = Context.User;
                    }
                    else
                    {
                        // The cookie authentication is listening for.
                        var identity = new ClaimsIdentity(defaultScheme);

                        // Adds claims with the SharePoint context CacheKey as issuer to the Identity object.
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.AuthenticationMethod, defaultScheme, null, claimIssuer),
                        };
                        identity.AddClaims(claims);
                        principal = new ClaimsPrincipal(identity);

                        // Handles the sign in method of the auth middleware.
                        await Context.Authentication.SignInAsync(cookieScheme, principal, authenticationProperties);  
                    }

                    // Creates the authentication ticket.
                    var ticket = new AuthenticationTicket(principal, authenticationProperties, cookieScheme);
                    result = AuthenticateResult.Success(ticket);
                    break;
                case RedirectionStatus.ShouldRedirect:
                    _redirectionStatus = RedirectionStatus.ShouldRedirect;

                    Response.StatusCode = 301;
                    result = AuthenticateResult.Failed("ShouldRedirect");

                    // Signs out so new signin to be performed on redirect back from SharePoint
                    await Context.Authentication.SignOutAsync(cookieScheme);

                    // Redirect to get new context token
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
