using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using APP.Common.Exception;
//using APP.Web.eCommerce.WebSite.API.Models;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class WebApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            base.OnException(context);

            if (context.Exception is BaseException)
            {/*
                var resMsg = new ResponseResult();
                resMsg.SetByResponseCode(ResponseCodes.Fail, context.Exception.Message);
                var resp = context.Request.CreateResponse(HttpStatusCode.OK, resMsg);
                context.Response = resp;*/
            }
            else
            {
                /*var resMsg = new ResponseResult();
                resMsg.SetByResponseCode(ResponseCodes.GeneralIntegrationError);
                var resp = context.Request.CreateResponse(HttpStatusCode.InternalServerError, resMsg);
                context.Response = resp;*/
            }
        }
    }
}