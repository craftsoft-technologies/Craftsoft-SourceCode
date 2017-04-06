using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace APP.Web.eCommerce.BLL
{
    public class StaticPagesCache : Dictionary<string, Dictionary<string, string>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture">return empty cache entity</param>
        /// <returns></returns>
        public Dictionary<string, string> AddCultureCache(string culture)
        {
            Debug.Assert(culture.ToLower() == culture, "culture should be lowercase");
            var cultureCache = new Dictionary<string, string>();
            this.Add(culture.ToLower(), cultureCache);
            return cultureCache;
        }

        public string GetPageHtml(string culture, string pageName)
        {
            Debug.Assert(culture.ToLower() == culture, "culture should be lowercase");
            Debug.Assert(pageName.ToLower() == pageName, "pageName should be lowercase");

            Dictionary<string, string> singleCultureDic = this[culture];
            if (singleCultureDic == null)
            {
                throw new Exception(string.Format("Can't find culture:{0} in StaticPagesCahe", culture));
            }
            string html = singleCultureDic[pageName];
            if (html == null)
            {
                throw new Exception(string.Format("Can't find pageName:{0} in StaticPagesCahe culture:{1}", pageName, culture));
            }
            return html;
        }
    }
}
