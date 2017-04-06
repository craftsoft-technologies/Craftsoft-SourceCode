using System;
using System.IO;
using System.Net;
using System.Collections.Concurrent;
using APP.Web.Common.Utilities;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.Mvc.Url
{
    public abstract class ContentCacheBase
    {
        readonly object syncRoot = new object();
        private readonly ConcurrentDictionary<string, string> _httpCache = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, string> _httpsCache = new ConcurrentDictionary<string, string>();

        private bool LocalPartnerFileExists(string contentPath)
        {
            bool result = false;

            string path = GetLocalPartnerPath(contentPath);
            result = File.Exists(path);

            return result;
        }

        internal string GetContentPath(string contentPath)
        {
            string path;
            path = LocalPartnerFileExists(contentPath) ? GetPartnerPath(contentPath) : GetBasePath(contentPath);
            path = string.Concat(path, "?v=", AppConfigManager.SystemSetting.CdnVers);
            return path;
        }

        protected abstract string GetPartnerPath(string contentPath);

        protected abstract string GetBasePath(string contentPath);

        protected abstract string GetLocalPartnerPath(string contentPath);
    }
}