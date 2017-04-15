using System;
using System.Web.Mvc;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class PublicControllerBase : MultiCultureControllerBase
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}