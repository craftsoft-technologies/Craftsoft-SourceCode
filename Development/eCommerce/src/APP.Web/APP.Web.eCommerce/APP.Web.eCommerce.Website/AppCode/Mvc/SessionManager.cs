using System;
using System.Web;
using APP.Web.eCommerce.BLL.Model;

namespace APP.Web.eCommerce.Website.Mvc
{
    public static class SessionManager
    {
        public static string SessionDataKey = "_Key";

        public static SessionData SetLogin(SessionData sessionData)
        {
            HttpContext.Current.Session[SessionDataKey] = sessionData;
            return sessionData;
        }

        public static SessionData SetLogout(string ip, string countryCode)
        {
            var sessionData = new SessionData(ip, countryCode);
            HttpContext.Current.Session[SessionDataKey] = sessionData;
            return sessionData;
        }

        public static SessionData Current
        {
            get
            {
                return HttpContext.Current.Session[SessionDataKey] as SessionData;
            }
        }
    }
}