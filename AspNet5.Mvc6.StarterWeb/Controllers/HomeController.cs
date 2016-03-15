using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Mvc;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Authentication;

namespace AspNet5.Mvc6.StarterWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISharePointConfiguration _sharePointConfiguration;
        public HomeController(ISharePointConfiguration sharePointConfiguration)
        {
            _sharePointConfiguration = sharePointConfiguration;
        }

        public IActionResult Index()
        {
            User spUser = null;
            var listTitles = new List<string>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var web = clientContext.Web;
                    clientContext.Load(web, w => w.Lists);
                    clientContext.ExecuteQuery();        
                    foreach (var list in web.Lists)
                    {
                        listTitles.Add(list.Title);
                    }
                }
            }

            ViewBag.UserName = HttpContext.User.GetUserName();
            ViewBag.Lists = listTitles;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            User spUser = null;
            var listTitles = new List<string>();

            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    var web = clientContext.Web;
                    clientContext.Load(web, w => w.Lists);
                    clientContext.ExecuteQuery();
                    foreach (var list in web.Lists)
                    {
                        listTitles.Add(list.Title);
                    }
                }
            }

            ViewBag.UserName = HttpContext.User.GetUserName();
            ViewBag.Lists = listTitles;

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
