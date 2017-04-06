using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class CultureConstraint : IRouteConstraint
    {
        public string ConstraintString { get; set; }

        private readonly Dictionary<string, bool> languageDic;

        public CultureConstraint(IEnumerable<LanguageInfo> languageInfos)
        {
            languageDic = new Dictionary<string, bool>();
            foreach (var languageInfo in languageInfos)
            {
                languageDic.Add(languageInfo.CultureName, true);
            }
            ConstraintString = string.Join(",", languageDic.Values);
        }

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // Get the value called "parameterName" from the 
            // RouteValueDictionary called "value"
            string value = values[parameterName].ToString();
            // Return true is the list of allowed values contains 
            // this value.
            return languageDic.ContainsKey(value);
        }

        #endregion
    }
}