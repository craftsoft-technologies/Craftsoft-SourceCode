using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json.Serialization;
using APP.Web.eCommerce.Website.AppCode.API;

namespace APP.Web.eCommerce.Website.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // disable XML output support from Web API
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;

            // customize Json.NET configuration
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Services.Replace(typeof(IExceptionLogger), new WebApiExceptionLogger());

            config.Filters.Add(new WebApiExceptionAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            SwaggerConfig.Register();
        }
    }
}