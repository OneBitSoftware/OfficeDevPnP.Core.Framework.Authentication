using Microsoft.AspNet.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using Microsoft.SharePoint.Client;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationHandler : AuthenticationHandler<SharePointAuthenticationOptions>
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Uri redirectUrl;
            var defaultSheme = "sharepoint";
            AuthenticateResult result = AuthenticateResult.Failed("Could not get the RedirectionStatus");
            
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    var principal = new ClaimsPrincipal();
                    var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
                    using (var clientContext = spContext.CreateUserClientContextForSPHost())
                    {
                        if (clientContext != null)
                        {
                            User spUser = null;
                            spUser = clientContext.Web.CurrentUser;
                            clientContext.Load(spUser, user => user.Title);
                            clientContext.ExecuteQuery();

                            GenericIdentity identity = new GenericIdentity(spUser.Title);
                            principal.AddIdentity(identity);

                            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), defaultSheme);
                            
                            //handle the sign in method of the auth middleware
                            await Context.Authentication.SignInAsync(defaultSheme, principal);

                            result = AuthenticateResult.Success(ticket);
                        }
                    }
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
            throw new NotImplementedException();
            return base.HandleUnauthorizedAsync(context);
        }

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            throw new NotImplementedException();
            return base.HandleSignOutAsync(context);
        }
    }
}
