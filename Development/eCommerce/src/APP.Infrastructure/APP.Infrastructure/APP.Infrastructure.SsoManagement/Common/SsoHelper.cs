using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using log4net;

namespace APP.Infrastructure.SsoManagement.Common
{
    public class SsoHelper
    {
        private ILog Logger = LogManager.GetLogger(typeof(SsoManager));

        #region Singleton

        private SsoHelper()
        {
            RetrieveConfigSessionInfo();
        }

        private static SsoHelper _instance;
        private static readonly object _locker = new object();
        public static SsoHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new SsoHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        private HttpCookieMode _cookieMode = HttpCookieMode.UseCookies;
        private string _cookieName = "ASP.NET_SessionId";

        private void RetrieveConfigSessionInfo()
        {
            var sessionStateSection = ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;
            if (sessionStateSection != null)
            {
                _cookieMode = sessionStateSection.Cookieless;
                _cookieName = sessionStateSection.CookieName;
            }
        }

        private bool IsCookieLess(HttpContext httpContext)
        {
            if (_cookieMode == HttpCookieMode.UseCookies)
                return false;
            else if (_cookieMode == HttpCookieMode.UseUri)
                return true;
            else
                return !httpContext.Request.Browser.Cookies;
        }

        private string GetCurrentUserName()
        {
            var user = HttpContext.Current.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                return user.Identity.Name;
            else
                return string.Empty;
        }

        private string GetSessionId()
        {
            var httpContext = HttpContext.Current;
            if (IsCookieLess(httpContext))
            {
                var matchCol = Regex.Matches(httpContext.Request.RawUrl, @"/\(S\((\w+?)\)\)/", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var captures = GetAllCaptures(matchCol).ToList();
                if (captures.Count >= 2)
                    return captures[1].Value;
            }
            else
            {
                var cookie = httpContext.Request.Cookies.Get(_cookieName);
                if (cookie != null)
                    return cookie.Value;
            }
            return string.Empty;
        }

        private IEnumerable<Capture> GetAllCaptures(MatchCollection mCol)
        {
            for (int i = 0; i < mCol.Count; i++)
            {
                for (int j = 0; j < mCol[i].Groups.Count; j++)
                {
                    for (int k = 0; k < mCol[i].Groups[j].Captures.Count; k++)
                    {
                        yield return mCol[i].Groups[j].Captures[k];
                    }
                }
            }
        }

        private string GetUserTokenKey(string userName)
        {
            return string.Format("{0}$#${1}", userName, GetSessionId());
        }

        public string GetTokenId()
        {
            var userName = GetCurrentUserName();
            var tokenKey = GetUserTokenKey(userName);

            var tokenId = HttpRuntime.Cache.Get(tokenKey) as string;

            Logger.Debug(string.Format("GetTokenId : Token Key {0}, Token Id : {1}", tokenKey, tokenId));

            return tokenId;
        }

        public void SetTokenId(string token)
        {
            var userName = GetCurrentUserName();

            SetTokenId(userName, token);
        }

        public void SetTokenId(string userName, string token)
        {
            var tokenKey = GetUserTokenKey(userName);
            HttpRuntime.Cache.Remove(tokenKey);
            HttpRuntime.Cache.Add(tokenKey, token, null,
                Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(HttpContext.Current.Session.Timeout),
                CacheItemPriority.NotRemovable, null);

            Logger.Debug(string.Format("SetTokenId : Token Key {0}, Token Id : {1}", tokenKey, token));
        }

        public static CultureInfo ResolveCulture()
        {
            string[] languages = HttpContext.Current.Request.UserLanguages;

            if (languages == null || languages.Length == 0)
                return null;

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static RegionInfo ResolveCountry()
        {
            CultureInfo culture = ResolveCulture();
            if (culture != null)
                return new RegionInfo(culture.LCID);

            return null;
        }
    }
}
