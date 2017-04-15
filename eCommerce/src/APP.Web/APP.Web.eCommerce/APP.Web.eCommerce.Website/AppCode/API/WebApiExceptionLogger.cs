using System;
using System.Web.Http.ExceptionHandling;
using APP.Common.Exception;
using APP.Common.Logger.Helper;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (!(context.Exception is BaseException))
            {
                LogHelper.User.Error(
                    string.Format("[API Exception] Timestamp: {0} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff")),
                    context.Exception.GetBaseException()
                );
            }
        }
    }
}