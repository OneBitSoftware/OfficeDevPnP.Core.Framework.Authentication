using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using Microsoft.SharePoint.Client;

namespace AspNet5.Mvc6.StarterWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            User spUser = null;

            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    if (clientContext != null)
            //    {
            //        spUser = clientContext.Web.CurrentUser;

            //        clientContext.Load(spUser, user => user.Title);

            //        clientContext.ExecuteQuery();

            //        ViewBag.UserName = spUser.Title;
            //    }
            //}

            ViewBag.UserName = HttpContext.User.GetUserName();
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
