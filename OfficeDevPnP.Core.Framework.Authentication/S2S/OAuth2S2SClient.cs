using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Authentication.S2S
{
    public class OAuth2S2SClient
    {
        public OAuth2S2SClient()
        {
        }

        public OAuth2Message Issue(string securityTokenServiceUrl, OAuth2AccessTokenRequest oauth2Request)
        {
            OAuth2Message oAuth2Message;
            OAuth2WebRequest oAuth2WebRequest = new OAuth2WebRequest(securityTokenServiceUrl, oauth2Request);
            try
            {
                WebResponse response = oAuth2WebRequest.GetResponse();
                oAuth2Message = OAuth2MessageFactory.CreateFromEncodedResponse(new StreamReader(response.GetResponseStream()));
            }
            catch (Exception exception)
            {
                throw new RequestFailedException("Token request failed.", exception);
            }
            return oAuth2Message;
        }
    }
}
