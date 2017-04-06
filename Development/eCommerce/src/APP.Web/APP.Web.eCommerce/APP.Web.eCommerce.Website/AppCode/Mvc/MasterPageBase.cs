using System;
using System.Web;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Web.eCommerce.BLL.Model;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class MasterPageBase : System.Web.Mvc.ViewMasterPage
    {
        public LanguageInfo LanguageInfo { get; set; }
        protected SessionData SessionData { get; set; }

        protected override void OnInit(EventArgs e)
        {
            SessionData = ViewBag.SessionData;

            #region Check preferred language from cookies
            LanguageInfo = ViewBag.LanguageInfo;

            HttpCookie ckLang = Request.Cookies["Lang"];

            if (ckLang != null)
            {
                if (LanguageInfo.CultureName != ckLang.Value)
                {
                    ckLang.Value = LanguageInfo.CultureName;
                    ckLang.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Set(ckLang); // update preferred language
                }
            }
            else
            {
                // create cookie to remember preferred language
                ckLang = new HttpCookie("Lang", LanguageInfo.CultureName);
                ckLang.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(ckLang);
            }
            #endregion

            base.OnInit(e);
        }
    }
}