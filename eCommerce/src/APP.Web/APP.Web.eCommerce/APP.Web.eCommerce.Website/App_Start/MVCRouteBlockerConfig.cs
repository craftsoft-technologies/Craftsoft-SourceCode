using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Web.Routing;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.App_Start
{
    public static class MVCRouteBlockerConfig
    {
        public static void Configure(RouteCollection routes)
        {
            if (AppConfigManager.PartnerFolderName == "eCommerceAPI")
            {
                routes.RemoveAll(s => !((Route)s).Url.Contains("swagger") && !((Route)s).Url.Contains("swagger/") && !((Route)s).Url.Contains("api/"));
            }
            else
            {
                routes.RemoveAll(s => ((Route)s).Url.Contains("swagger") || ((Route)s).Url.Contains("swagger/") || ((Route)s).Url.Contains("api/"));
            }
        }

        public static void RemoveAll<T>(this Collection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }
        }
    }
}