using Microsoft.AspNet.Authentication;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationHandler : AuthenticationHandler<SharePointAuthenticationOptions>
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Uri redirectUrl;
            var defaultSheme = SharePointAuthenticationDefaults.AuthenticationScheme;
            AuthenticateResult result = AuthenticateResult.Failed("Could not get the RedirectionStatus");

            //setup the SharePoint configuration based on the middleware options
            SharePointContextProvider.GetInstance(SharePointConfiguration.GetFromSharePointAuthenticationOptions(Options));
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    //get instance of the SharePointAcsContextProvider in the SharePointContextProvider.Current property
                    var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
                    var principal = new ClaimsPrincipal();

                    GenericIdentity identity = new GenericIdentity(spContext.UserAccessTokenForSPHost);
                    principal.AddIdentity(identity);

                    var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), defaultSheme);
                    //handle the sign in method of the auth middleware
                    await Context.Authentication.SignInAsync(defaultSheme, principal);
                    result = AuthenticateResult.Success(ticket);
                    break;
                case RedirectionStatus.ShouldRedirect:
                    //filterContext.Result = new RedirectResult(redirectUrl.AbsoluteUri); //TODO: should I investigate
                    result = AuthenticateResult.Failed("ShouldRedirect");
                    break;
                case RedirectionStatus.CanNotRedirect:
                    result = AuthenticateResult.Failed("CanNotRedirect");
                    break;
            }
            return result;
        }

        protected override async Task HandleSignInAsync(SignInContext context)
        {
            await base.HandleSignInAsync(context);
            SignInAccepted = true;

            //throw new NotImplementedException();
            //return base.HandleSignInAsync(context);
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            //throw new NotImplementedException();
            return base.HandleUnauthorizedAsync(context);
        }

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            throw new NotImplementedException();
            return base.HandleSignOutAsync(context);
        }
    }
}
