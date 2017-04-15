using System.Web.Mvc;
using APP.Web.eCommerce.Website.Mvc;

namespace APP.Web.eCommerce.Website.Controllers
{
    public class ErrorController : PublicControllerBase
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return CreateActionInvoker(false);
        }

        public virtual ActionResult General()
        {
            return View();
        }

        public virtual ActionResult _401()
        {
            return View();
        }

        public virtual ActionResult _404()
        {
            return View();
        }

        public virtual ActionResult _403()
        {
            return View();
        }

        public virtual ActionResult TestMonitor()
        {
            return View();
        }

        public virtual ActionResult TestError()
        {
            return View();
        }
    }
}