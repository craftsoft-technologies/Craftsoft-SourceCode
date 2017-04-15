using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using APP.Web.Common.Utilities;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Web.eCommerce.Website.App_GlobalResources;

namespace APP.Web.eCommerce.Website.AppCode
{
    public static class StaticContentManager
    {
        //static readonly object StaticContentSyncRoot = new object();
        private static readonly Regex reg = new Regex(@"({\w*})");

        public static string GetContent(LanguageInfo languageInfo, string fileName)
        {
            var baseStaticFile =
                HttpContext.Current.Server.MapPath(string.Format(@"\StaticPages\{0}\{1}", languageInfo.CultureName,
                                                                 fileName));
            var partnerStaticFile =
                HttpContext.Current.Server.MapPath(string.Format(@"\_Partners\{0}\StaticPages\{1}\{2}",
                                                                 AppConfigManager.PartnerFolderName,
                                                                 languageInfo.CultureName, fileName));
            string existFile = File.Exists(partnerStaticFile) ? partnerStaticFile : baseStaticFile;
            LogHelper.Server.InfoFormat(" Load Static Content From: {0}", existFile);
            string content = LoadStaticContentString(existFile, languageInfo.Culture);
            return content;
        }

        private static string LoadStaticContentString(string fileName, CultureInfo culture)
        {
            var tempUICulture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = culture;

                string content = File.ReadAllText(fileName);
                foreach (Match match in reg.Matches(content)) //{SiteName}
                {
                    string key = match.Value.Trim(new[] { '{', '}' });
                    string value = "";//Text.ResourceManager.GetString(key);
                    if (!string.IsNullOrEmpty(value))
                    {
                        content = content.Replace(match.Value, value);
                    }
                }
                return content;
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = tempUICulture;
            }
        }
    }
}