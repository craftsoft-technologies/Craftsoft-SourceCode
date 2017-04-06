using System;
using System.Web;
using System.Web.Mvc;

namespace APP.Web.eCommerce.Website
{
    public partial class CookieHelper
    {
        HttpCookie _cookie;
        string _lastCookieName;

        static HttpContext context
        {
            get { return HttpContext.Current; }
        }

        public HttpCookie Current
        {
            get { return this._cookie; }
        }

        public string LastCookieName
        {
            get { return this._lastCookieName; }
        }

        public CookieHelper(string cookieName)
        {
            this._cookie = context.Request.Cookies[_lastCookieName = cookieName];
        }
    }

    public partial class CookieHelper
    {
        public void Create(string cookieName, object cookieValue, DateTime expirationDate)
        {
            HttpCookie cookie;

            if (!Exists(cookieName, out cookie))
            {
                OnSetCookie(cookie = new HttpCookie(cookieName)
                {
                    Value = Convert.ToString(cookieValue),
                    Expires = expirationDate
                });
            }
        }
        public void Create(HttpCookie cookie)
        {
            if (cookie != null)
            {
                this.Create(cookie.Name, cookie.Value, cookie.Expires);
            }
        }

        public bool Exists(string cookieName, out HttpCookie cookie)
        {
            return (cookie = context.Request.Cookies[cookieName]) != null;
        }
        public bool Exists(string cookieName)
        {
            HttpCookie t;
            return this.Exists(cookieName, out t);
        }
        public bool Exists()
        {
            return Current != null;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        protected void OnSetCookie(HttpCookie attemptedObject)
        {
            if (attemptedObject != null)
            {
                _lastCookieName = attemptedObject.Name;
                context.Response.Cookies.Add(_cookie = attemptedObject);
            }
        }

        public virtual void SynchronizeProxyCookie(object value, object page)
        {
            if (LastCookieName == null)
                return;

            HttpCookie proxyCookie;

            if (!Exists(LastCookieName, out proxyCookie))
            {
#if RELEASE
                const string PROXY_UID_PREFIX = "__Xuenn.Aff.";

                string sid = CryptoHelper.SHA256Crypto(CommonHelper.GetClientIP(), PROXY_UID_PREFIX);
                string proxyHost = ConfigurationManager<GCT.Manager.Integration.Configuration.AppConfiguration>.Instance.casinoConfig.AffCookieMgmtHost;
#else
                string sid = string.Empty;
                string proxyHost = "api.dev";
#endif

                var apiUri = new UriBuilder()
                {
                    Scheme = Uri.UriSchemeHttp,
                    Host = proxyHost,
                    Path = "spi/afc",
                    Query = HttpUtility.UrlEncode(String.Format(
                        "key={0}&value={1}&sid={2}&callback={3}",
                        LastCookieName, value ?? string.Empty, sid, "cookify"
                    ))
                };

                var controller = page as ControllerBase;

                if (controller != null)
                {
                    controller.ViewBag.ProxyCookieUrl = apiUri.ToString();
                }
                else
                {
                    (page as ViewMasterPage).ViewBag.ProxyCookieUrl = apiUri.ToString();
                }
            }
        }

    }
}