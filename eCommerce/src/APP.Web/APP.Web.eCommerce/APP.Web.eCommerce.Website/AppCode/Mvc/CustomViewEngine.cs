using System.Web.Mvc;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class CustomViewEngine : RazorViewEngine
    {
        public string[] ViewLocations { get; set; }

        public CustomViewEngine(string partnerName)
        {
            string[] viewLocations;
            if (string.IsNullOrEmpty(partnerName))
            {
                viewLocations = new[] {
                                          "~/Views/{1}/{0}.cshtml",
                                          "~/Views/Shared/{0}.cshtml"
                                      };
            }
            else
            {
                viewLocations = new[] {
                                          "~/_Partners/"+partnerName+"/Views/{1}/{0}.cshtml",
                                          "~/_Partners/"+partnerName+"/Views/Shared/{0}.cshtml",
                                          "~/Views/{1}/{0}.cshtml",
                                          "~/Views/Shared/{0}.cshtml"
                                      };
            }

            ViewLocations = viewLocations;
            PartialViewLocationFormats = viewLocations;
            ViewLocationFormats = viewLocations;
        }
    }
}