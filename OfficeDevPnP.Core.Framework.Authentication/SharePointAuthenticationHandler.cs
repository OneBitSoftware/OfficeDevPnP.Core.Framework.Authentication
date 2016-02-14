using Microsoft.AspNet.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Features.Authentication;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationHandler : AuthenticationHandler<SharePointAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
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
