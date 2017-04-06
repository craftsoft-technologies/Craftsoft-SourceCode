using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class SwitchCultureActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            CultureInfo cultureEn = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = cultureEn;

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            CultureInfo cultureEn = new CultureInfo(AppConfigManager.LanguageInfos[0].CultureName);
            Thread.CurrentThread.CurrentCulture = cultureEn;

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}