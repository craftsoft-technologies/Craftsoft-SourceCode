using System;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using APP.Common.Utilities;

namespace APP.Web.Common.Mvc.ActionResults
{
    public class JsonDataContractResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                DataContractJsonSerializer serializer = JsonSerializerHelper.GetSerializer(this.Data.GetType());
                serializer.WriteObject(response.OutputStream, this.Data);
            }
        }
    }
}
