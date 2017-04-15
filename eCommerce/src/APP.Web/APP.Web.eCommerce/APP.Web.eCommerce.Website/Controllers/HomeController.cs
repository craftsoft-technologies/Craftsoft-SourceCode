using System.Web.Mvc;
using APP.Web.eCommerce.Website.Mvc;

namespace APP.Web.eCommerce.Website.Controllers
{
    public class HomeController : PublicControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}