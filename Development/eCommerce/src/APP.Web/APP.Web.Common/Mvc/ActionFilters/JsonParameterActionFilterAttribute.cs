using System;
using System.IO;
using System.Web.Mvc;
using APP.Common.Utilities;

namespace APP.Web.Common.Mvc.ActionFilters
{
    public class JsonParameterActionFilterAttribute : ActionFilterAttribute
    {
        public string Param { get; set; }
        public Type RootType { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                // not JSON request
                return;
            }

            int length = (int)filterContext.HttpContext.Request.InputStream.Length;
            if (length == 0)
            {
                return;
            }

            byte[] buffer = new byte[length];
            filterContext.HttpContext.Request.InputStream.Read(buffer, 0, length);
            filterContext.HttpContext.Request.InputStream.Position = 0; // reset stream position

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                filterContext.ActionParameters[Param] = JsonSerializerHelper.GetSerializer(RootType).ReadObject(ms);
            }
        }
    }
}
