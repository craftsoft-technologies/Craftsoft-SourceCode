using System;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.Mvc.Url
{
    internal class PartnerDesignContentCache : ContentCacheBase
    {
        protected override string GetPartnerPath(string contentPath)
        {
            string result = string.Concat(AppConfigManager.SystemSetting.Cdn, "/", AppConfigManager.PartnerSetting.CdnPartnerFolder, "/", AppConfigManager.PartnerSetting.CdnDesignFolder, "/", contentPath);

            return result;
        }

        protected override string GetBasePath(string contentPath)
        {
            string result = string.Concat(AppConfigManager.SystemSetting.Cdn, "/", "base", "/", AppConfigManager.PartnerSetting.CdnDesignFolder, "/", contentPath);

            return result;
        }

        protected override string GetLocalPartnerPath(string contentPath)
        {
            string result = string.Concat(System.Web.HttpContext.Current.Server.MapPath("~"), "cdn\\", AppConfigManager.PartnerSetting.CdnPartnerFolder, "\\", AppConfigManager.PartnerSetting.CdnDesignFolder, "\\", contentPath.Replace("/", "\\"));

            return result;
        }
    }
}