using System.Threading;
using System.Web.Mvc;
using APP.Web.eCommerce.Website.Mvc.Url;

namespace APP.Web.eCommerce.Website.Mvc
{
    public static class UrlExtension
    {
        private static readonly PartnerContentCache partnerContentCache = new PartnerContentCache();
        private static readonly PartnerDesignContentCache partnerDesignContentCache = new PartnerDesignContentCache();

        public static string PartnerContent(this UrlHelper url, string contentPath)
        {
            return partnerContentCache.GetContentPath(contentPath);
        }

        public static string PartnerDesignContent(this UrlHelper url, string contentPath)
        {
            return partnerDesignContentCache.GetContentPath(contentPath);
        }

        public static string CultureRoute(this UrlHelper url, string action)
        {
            string cultureName = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            return url.RouteUrl("CultureRoute", new { Action = action, Culture = cultureName });
        }

        public static string CultureRoute(this UrlHelper url, string action, string controller)
        {
            string cultureName = Thread.CurrentThread.CurrentCulture.Name.ToLower();
            var routeValues = new { Action = action, Controller = controller, Culture = cultureName };
            return url.RouteUrl("CultureRoute", routeValues);
        }
    }
}