using Microsoft.AspNet.Authentication;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using System.Linq;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationHandler : AuthenticationHandler<SharePointAuthenticationOptions>
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Uri redirectUrl;
            var defaultScheme = SharePointAuthenticationDefaults.AuthenticationScheme;
            AuthenticateResult result = AuthenticateResult.Failed("Could not get the RedirectionStatus");

            //setup the SharePoint configuration based on the middleware options
            SharePointContextProvider.GetInstance(SharePointConfiguration.GetFromSharePointAuthenticationOptions(Options));
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    //check if we already have authenticated principal
                    ClaimsPrincipal principal;
                    if (Context.User.Identities.Any(identity => identity.IsAuthenticated)) //TODO: IsAuthenticated is awlays false. To be decided wheather and how we presist the user (Context.User) state. We may not need to do it if we follow the SharePointContextProvider concept that handles context details in session.
                    {
                        principal = Context.User;
                    }
                    else
                    {
                        // the cookie authentication is listening for
                        var identity = new ClaimsIdentity(defaultScheme);

                        //Add empty claims to the Identity object
                        var newClaims = new[]
                        {
                            new Claim(ClaimTypes.AuthenticationMethod, defaultScheme),
                        };
                        identity.AddClaims(newClaims);

                        // create the authentication ticket
                        principal = new ClaimsPrincipal(identity);

                        principal.AddIdentity(identity);
                        //handle the sign in method of the auth middleware
                        await Context.Authentication.SignInAsync(defaultScheme, principal, new AuthenticationProperties()
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                            IsPersistent = false,
                            AllowRefresh = false
                        });
                    }
                    var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), defaultScheme);
                    result = AuthenticateResult.Success(ticket);
                    break;
                case RedirectionStatus.ShouldRedirect:
                    Response.StatusCode = 301;
                    result = AuthenticateResult.Failed("ShouldRedirect");
                    Context.Response.Redirect(redirectUrl.AbsoluteUri);
                    break;
                case RedirectionStatus.CanNotRedirect:
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

        protected override async Task HandleSignOutAsync(SignOutContext context)
        {
            await Context.Authentication.SignOutAsync(SharePointAuthenticationDefaults.AuthenticationScheme);
            await base.HandleSignOutAsync(context);
        }
    }
}
