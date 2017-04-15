using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using APP.Common.Utilities;
using APP.Web.Common.Utilities;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Web.eCommerce.Website.Mvc;

namespace APP.Web.eCommerce.Website
{
    public static class GlobalHelper
    {
        public static DateTime ServerStartUpTime { get; set; }
        public static int SessionCount { get; set; }

        static readonly object ConfigurationSyncRoot = new object();

        public static string GetFirstExistFileName(string partnerFolder, string baseFolder, string fileName)
        {
            return File.Exists(partnerFolder + fileName) ? partnerFolder + fileName : baseFolder + fileName;
        }

        private static T ToObj<T>(string partnerFolder, string baseFolder, string fileName)
        {
            string existFileFullName = GetFirstExistFileName(partnerFolder, baseFolder, fileName);
            LogHelper.Server.InfoFormat(" Loading... [{0}]", existFileFullName);
            string xml = File.ReadAllText(existFileFullName);
            return XmlSerializerHelper.ToObj<T>(xml);
        }

        private static string GetFolder(string partnerFolder, string baseFolder, string fileName)
        {
            string folderName;
            if (File.Exists(partnerFolder + fileName))
            {
                folderName = partnerFolder;
            }
            else
            {
                folderName = baseFolder;
            }
            LogHelper.Server.InfoFormat(" Loading... [{0}] from [{1}]", fileName, folderName);
            return folderName;
        }

        public static void InitializeConfiguration(string partnerConfigurationFolder, string baseConfigurationFolder)
        {
            LoadConfiguration(partnerConfigurationFolder, baseConfigurationFolder);
            new FileWatcherHelper(partnerConfigurationFolder, () => LoadConfiguration(partnerConfigurationFolder, baseConfigurationFolder)).Start();
            new FileWatcherHelper(baseConfigurationFolder, () => LoadConfiguration(partnerConfigurationFolder, baseConfigurationFolder)).Start();
            LogHelper.Server.InfoFormat(" Added FileWatcher for partner:[{0}] base:[{1}]", partnerConfigurationFolder, baseConfigurationFolder);
        }

        private static void LoadConfiguration(string partnerConfigurationFolder, string baseConfigurationFolder)
        {
            lock (ConfigurationSyncRoot)
            {
                LogHelper.Server.InfoFormat(" Load Configuration: partner:[{0}] base:[{1}]", partnerConfigurationFolder, baseConfigurationFolder);

                string partnerRefFolder = partnerConfigurationFolder + "Ref\\";
                string baseRefFolder = baseConfigurationFolder + "Ref\\";

                Infrastructure.ConnectionStringManagement.AppConfiguration.Initialize(GetFolder(partnerRefFolder, baseRefFolder, Infrastructure.ConnectionStringManagement.AppConfiguration.ConfigFileName));
                Infrastructure.EmailManagement.Configuration.AppConfiguration.Initialize(GetFolder(partnerRefFolder, baseRefFolder, Infrastructure.EmailManagement.Configuration.AppConfiguration.ConfigFileName));
                Infrastructure.SsoManagement.Configuration.AppConfiguration.Initialize(GetFolder(partnerRefFolder, baseRefFolder, Infrastructure.SsoManagement.Configuration.AppConfiguration.ConfigFileName));
                Manager.Common.FileManager.AppConfiguration.Initialize(GetFolder(partnerRefFolder, baseRefFolder, Manager.Common.FileManager.AppConfiguration.ConfigFileName));
                DropdownListUtil.Load(GetFirstExistFileName(partnerRefFolder, baseRefFolder, "DropdownListEntity.config"));
                AppConfigManager.LanguageInfos = ToObj<List<LanguageInfo>>(partnerConfigurationFolder, baseConfigurationFolder, "LanguageInfo.config");
                AppConfigManager.DomainInfos = ToObj<List<DomainInfo>>(partnerConfigurationFolder, baseConfigurationFolder, "DomainInfo.config");
                AppConfigManager.RedirectSetting = ToObj<RedirectSettings>(partnerConfigurationFolder, baseConfigurationFolder, "RedirectSetting.config");
                AppConfigManager.CountryDefaultSettings = ToObj<CountryDefaultSettings>(partnerConfigurationFolder, baseConfigurationFolder, "CountryDefaultSetting.config");
                AppConfigManager.SystemSetting = ToObj<SystemSetting>(partnerConfigurationFolder, baseConfigurationFolder, "SystemSetting.config");
                AppConfigManager.PartnerSetting = ToObj<PartnerSetting>(partnerConfigurationFolder, baseConfigurationFolder, "PartnerSetting.config");
                AppConfigManager.SiteHostRedirect = ToObj<List<SiteHostRedirect>>(partnerConfigurationFolder, baseConfigurationFolder, "SiteHostRedirect.config");
                AppConfigManager.PartnerRedirect = ToObj<List<PartnerRedirect>>(partnerConfigurationFolder, baseConfigurationFolder, "PartnerRedirectSetting.config");
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Empty",
                url: string.Empty,
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            var cultureRoute = routes.MapRoute(
                name: "CultureRoute",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional }
            );


            LogHelper.Server.InfoFormat(" Map Culture Route:{0}", cultureRoute.Url);

            var cultureConstraint = new CultureConstraint(AppConfigManager.LanguageInfos);
            cultureRoute.Constraints.Add("culture", cultureConstraint);

            LogHelper.Server.InfoFormat(" Route Constraints:{0}", cultureConstraint.ConstraintString);
        }

        public static void InitViewEngine(string partnerNamespace)
        {
            ViewEngines.Engines.Clear();
            var viewEngine = new CustomViewEngine(partnerNamespace);
            LogHelper.Server.InfoFormat(" View Locations:{0}", string.Join(",", viewEngine.ViewLocations));
            ViewEngines.Engines.Add(viewEngine);
        }

        public static decimal SetDecimalByLanguage(string value)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            switch (cultureInfo.ToString().ToLower())
            {
                case "id":
                    string[] arr = value.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    decimal num = decimal.Parse(arr[0]);
                    return num;
                default:
                    return decimal.Parse(value);
            }
        }

        public static bool IsAdvancedPage(object oBrowser)
        {
            var browser = System.Web.HttpContext.Current.Request.Browser;
            var useragent = System.Web.HttpContext.Current.Request.UserAgent;
            bool advancedPage = true;
            bool isMobile = useragent.ToLower().Contains("mobile");
            bool isTablet = (useragent.ToLower().Contains("android") && !useragent.ToLower().Contains("mobile")) || useragent.ToLower().Contains("tablet") || useragent.ToLower().Contains("ipad");
            float bv = -1;
            if (browser.Browser == "IE")
            {
                bv = (float)(browser.MajorVersion + browser.MinorVersion);

                if (bv <= 10)
                {
                    advancedPage = false;
                }
            }

            if (isMobile)
            {
                advancedPage = false;
            }

            if (isTablet)
            {
                advancedPage = true;
            }

            return advancedPage;
        }

        public static bool IsTargetDate()
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN");
            DateTime targetDate = DateTime.Parse(AppConfigManager.PartnerSetting.ScheduleDate);
            DateTime currentDate = DateTime.Now;
            bool isTargetDateReached = false;

            if (targetDate > currentDate)
            {

                isTargetDateReached = false;
            }
            else
            {
                isTargetDateReached = true;
            }
            return isTargetDateReached;
        }
    }
}