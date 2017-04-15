using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APP.Infrastructure.SsoManagement.Common;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Web.eCommerce.Website.Mvc;
using APP.Web.eCommerce.BLL;

namespace APP.Web.eCommerce.Website.Controllers
{
    public class DefaultController : PublicControllerBase
    {
        public virtual ActionResult Index()
        {
            string language;

            HttpCookie cookie = Request.Cookies["Lang"];
            string ip = CommonHelper.GetClientIP(System.Web.HttpContext.Current.Request);
            string countryCode = ip; //!= "::1" ? SecurityManager.GetCountryCodeByIp(ip) : "ID"; // default to Indonesia

            string languageCodeIP = "ID";//MemberManager.Instance.GetMemberIPLanguageMapping(countryCode);

            if (!string.IsNullOrEmpty(languageCodeIP))
            {
                language = languageCodeIP;
            }
            else
            {
                if (cookie != null)
                {
                    language = cookie.Value.ToString();

                    // double check again the language in cookie is supported or not. Because local development different partner support different languages
                    var list = from l in AppConfigManager.LanguageInfos
                               where l.CultureName == language
                               select l;

                    if (list.Count() == 0)
                        language = AppConfigManager.LanguageInfos[0].CultureName; // take the first language as default
                }
                else
                {
                    language = AppConfigManager.LanguageInfos[0].CultureName; // take the first language as default

                    // create cookie to remember preferred language
                    cookie = new HttpCookie("Lang", language);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(cookie);
                }
            }

            return Redirect(language + "//");
        }
    }
}