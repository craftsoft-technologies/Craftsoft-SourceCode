using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Web.eCommerce.BLL.Configuration
{
    public class PartnerSetting
    {
        public string Namespace { get; set; }
        public string CdnPartnerFolder { get; set; }
        public string CdnDesignFolder { get; set; }
        public string IsUseExternalCDN { get; set; }
        public string CdnExternalUrl { get; set; }
        public string GoogleAnalytics { get; set; }
        public string FiveThreeKFAnalytics { get; set; }
        public string MerchantId { get; set; }
        public bool AllowSendSignUpWelcomeEmail { get; set; }
        public bool UseCaptcha { get; set; }
        public string PartnerStaticFileRemote { get; set; }
        public bool RedirectToDesktopSite { get; set; }
        public string DesktopSiteURL { get; set; }
        public string WebTraffic { get; set; }
        public string ScheduleDate { get; set; }
        public string DrawDateCdnFilePath { get; set; }
        public double DrawDateCacheExpirationInMinute { get; set; }
        public double DrawDateShowTimeOffsetInHour { get; set; }
    }
}
