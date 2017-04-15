using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Web.eCommerce.BLL.Configuration
{
    public static class AppConfigManager
    {
        public static List<LanguageInfo> LanguageInfos { get; set; }
        public static List<DomainInfo> DomainInfos { get; set; }
        public static SystemSetting SystemSetting { get; set; }
        public static PartnerSetting PartnerSetting { get; set; }
        public static RedirectSettings RedirectSetting { get; set; }
        public static List<MvcLinkInfo> CorporateAffairsNavigation { get; set; }
        public static List<MvcLinkInfo> TutorialsNavigation { get; set; }
        public static List<MvcLinkInfo> InfoCenterNavigation { get; set; }
        public static List<SiteHostRedirect> SiteHostRedirect { get; set; }
        public static StaticPagesCache StaticPages { get; set; }
        public static CountryDefaultSettings CountryDefaultSettings { get; set; }
        public static string PartnerFolderName { get; set; }
        public static List<PartnerRedirect> PartnerRedirect { get; set; }
        public static List<BmtPageContent> BmtPageContents { get; set; }
    }
}
