using Microsoft.AspNet.Mvc;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Authentication;

namespace AspNet5.Mvc6.StarterWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var auth = this.HttpContext.User.Identity.IsAuthenticated;
            User spUser = null;
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;

                    clientContext.Load(spUser, user => user.Title);

                    clientContext.ExecuteQuery();

                    ViewBag.UserName = spUser.Title;
                }
            }
            return View();
        }

        public IActionResult About()
        {
            var auth = this.HttpContext.User.Identity.IsAuthenticated;
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
