using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APP.Web.eCommerce.Website.Mvc;

namespace APP.Web.eCommerce.Website._Partners.Noise.Controllers
{
    public class HomeController : PublicControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}