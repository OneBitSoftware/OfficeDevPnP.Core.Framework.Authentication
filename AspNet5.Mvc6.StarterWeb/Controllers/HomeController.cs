using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using Microsoft.SharePoint.Client;

using OfficeDevPnP.Core.Framework.Authentication;

namespace AspNet5.Mvc6.StarterWeb.Controllers
{
    public class HomeController : Controller
    {
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
