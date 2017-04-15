using System.Diagnostics;
using System.Web.Mvc;
using APP.Web.Common.Utilities;

namespace APP.Web.Common.Mvc.ActionFilters
{
    public class PerformanceLogActionFilterAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch stopWatch = new Stopwatch();

        public PerformanceLogActionFilterAttribute()
        {
            stopWatch.Start();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            stopWatch.Stop();
            LogHelper.RequestPerformance(stopWatch.Elapsed, filterContext.HttpContext.Request.RawUrl);
        }
    }
}
