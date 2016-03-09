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

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationHandler : AuthenticationHandler<SharePointAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            AuthenticateResult result = AuthenticateResult.Failed("Could not get the RedirectionStatus");
            //var abstractContext = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);
            Uri redirectUrl;
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    var principal = new ClaimsPrincipal();
                    var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
                    using (var clientContext = spContext.CreateUserClientContextForSPHost())
                    {
                        if (clientContext != null)
                        {
                            GenericIdentity identity = new GenericIdentity(clientContext.Web.CurrentUser.LoginName);
                            //clientContext.Load(spUser, user => user.Title);
                            //clientContext.ExecuteQuery();
                            //ViewBag.UserName = spUser.Title;
                            principal.AddIdentity(identity);
                            result =
                                AuthenticateResult.Success(new AuthenticationTicket(principal,
                                    new AuthenticationProperties(), "sharepoint"));
                        }
                    }
                    break;
                case RedirectionStatus.ShouldRedirect:
                    //filterContext.Result = new RedirectResult(redirectUrl.AbsoluteUri);
                    result = AuthenticateResult.Failed("ShouldRedirect");
                    break;
                case RedirectionStatus.CanNotRedirect:
                    result = AuthenticateResult.Failed("CanNotRedirect");
                    break;
            }
            return new Task<AuthenticateResult>(() => result);
        }

        protected override Task HandleSignInAsync(SignInContext context)
        {
            throw new NotImplementedException();
            return base.HandleSignInAsync(context);
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
