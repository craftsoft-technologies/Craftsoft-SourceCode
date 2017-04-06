using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;

namespace APP.Web.Common.Mvc.ActionFilters
{
    public enum CachePolicy
    {
        NoCache = 0,
        Client = 1,
        Server = 2,
        ClientAndServer = 3
    }

    public class CacheFilterAttribute : ActionFilterAttribute
    {
        #region Public properties

        public int Duration { get; set; }
        public string VaryByParam { get; set; }
        public CachePolicy CachePolicy { get; set; }

        #endregion

        #region Private members
        private static MethodInfo _switchWriterMethod = typeof(HttpResponse).GetMethod("SwitchWriter", BindingFlags.Instance | BindingFlags.NonPublic);
        private TextWriter _originalWriter;
        private string _cacheKey;
        #endregion

        #region ActionFilterAttribute overrides

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Client-side caching?
            if (CachePolicy == CachePolicy.Client || CachePolicy == CachePolicy.ClientAndServer)
            {
                if (Duration <= 0) return;

                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetExpires(DateTime.Now.Add(cacheDuration));
                cache.SetMaxAge(cacheDuration);
                cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Server-side caching?
            if (CachePolicy == CachePolicy.Server || CachePolicy == CachePolicy.ClientAndServer)
            {
                _cacheKey = ComputeCacheKey(filterContext);
                string cachedOutput = (string)filterContext.HttpContext.Cache[_cacheKey];
                if (cachedOutput != null)
                    filterContext.Result = new ContentResult { Content = cachedOutput };
                else
                    _originalWriter = (TextWriter)_switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { new HtmlTextWriter(new StringWriter()) });
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Server-side caching?
            if (CachePolicy == CachePolicy.Server || CachePolicy == CachePolicy.ClientAndServer)
            {
                if (_originalWriter != null)
                {
                    // Must complete the caching

                    HtmlTextWriter cacheWriter = (HtmlTextWriter)_switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { _originalWriter });
                    string textWritten = ((StringWriter)cacheWriter.InnerWriter).ToString();
                    filterContext.HttpContext.Response.Write(textWritten);
                    filterContext.HttpContext.Cache.Add(_cacheKey, textWritten, null, DateTime.Now.AddSeconds(Duration), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }

            }
        }

        #endregion

        #region Helper methods

        private string ComputeCacheKey(ActionExecutingContext filterContext)
        {
            StringBuilder cacheKey = new StringBuilder();

            // Controller + action
            cacheKey.Append(filterContext.Controller.GetType().FullName);
            if (filterContext.RouteData.Values.ContainsKey("action"))
            {
                cacheKey.Append("_");
                cacheKey.Append(filterContext.RouteData.Values["action"].ToString());
            }

            // Variation by parameters
            List<string> varyByParam = VaryByParam.Split(';').ToList();

            if (!string.IsNullOrEmpty(VaryByParam))
            {
                foreach (KeyValuePair<string, object> pair in filterContext.RouteData.Values)
                {
                    if (VaryByParam == "*" || varyByParam.Contains(pair.Key))
                    {
                        cacheKey.Append("_");
                        cacheKey.Append(pair.Key);
                        cacheKey.Append("=");
                        cacheKey.Append(pair.Value.ToString());
                    }
                }
            }

            return cacheKey.ToString();
        }


        #endregion

    }

    public class NonClientCacheFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}
