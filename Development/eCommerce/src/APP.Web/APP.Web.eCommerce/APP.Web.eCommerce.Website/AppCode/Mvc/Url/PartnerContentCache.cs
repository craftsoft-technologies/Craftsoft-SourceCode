using System;
using System.Web;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.Mvc.Url
{
    internal class PartnerContentCache : ContentCacheBase
    {
        protected override string GetPartnerPath(string contentPath)
        {
            string result = string.Empty;

            result = string.Concat(AppConfigManager.SystemSetting.Cdn, "/", AppConfigManager.PartnerSetting.CdnPartnerFolder, "/", contentPath);

            return result;

        }

        protected override string GetBasePath(string contentPath)
        {
            string result = string.Empty;

            result = string.Concat(AppConfigManager.SystemSetting.Cdn, "/", "base", "/", contentPath);

            return result;

        }

        protected override string GetLocalPartnerPath(string contentPath)
        {
            string result = string.Empty;

            result = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~"), "cdn\\", AppConfigManager.PartnerSetting.CdnPartnerFolder, "\\", contentPath.Replace("/", "\\"));

            return result;
        }
    }
}