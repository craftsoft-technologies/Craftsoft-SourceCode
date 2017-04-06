using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APP.Web.eCommerce.Website.Mvc;
using APP.Web.eCommerce.Website.Models;

namespace APP.Web.eCommerce.Website._Partners.Demo.Controllers
{
    public class HomeController : PublicControllerBase
    {
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel()
            {
                Home = ""
            };
            return View(model);
        }
    }
}