using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class LanguageControllerFactory : IControllerFactory
    {
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();
        private static readonly object TypeCacheSyncRoot = new object();
        private const string ControlerNamespace1 = "APP.Web.eCommerce.Website._Partners.{1}.Controllers.{0}Controller";
        private const string ControlerNamespace2 = "APP.Web.eCommerce.Website.Controllers.{0}Controller";
        private const string ControlerNamespace3 = "APP.Web.eCommerce.Website._Partners.{1}.Controllers.Services.{0}";
        private const string ControlerNamespace4 = "APP.Web.eCommerce.Website.Controllers.Services.{0}";

        private static IController CreateController(RequestContext context, Type controllerType)
        {
            try
            {
                if (context.RouteData.Values["culture"] != null)
                {
                    string culture = context.RouteData.Values["culture"].ToString();
                    if (!string.IsNullOrEmpty(culture))
                    {
                        var language = AppConfigManager.LanguageInfos.First(l => l.CultureName.Equals(culture, StringComparison.InvariantCultureIgnoreCase));
                        Thread currentThread = Thread.CurrentThread;
                        currentThread.CurrentCulture = language.Culture;
                        currentThread.CurrentUICulture = language.Culture;

                        MultiCultureControllerBase controller;
                        try
                        {
                            controller = (MultiCultureControllerBase)Activator.CreateInstance(controllerType);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Please make sure controler inherit from ServiceControlerBase or PageControlerBase", ex);
                        }

                        controller.LanguageInfo = language;
                        return controller;
                    }
                }
                return (IController)Activator.CreateInstance(controllerType);
            }
            catch (ArgumentNullException)
            {
                throw new HttpException(404, "");
            }
        }

        IController IControllerFactory.CreateController(RequestContext context, string controllerName)
        {
            Type controllerType;
            if (!TypeCache.TryGetValue(controllerName, out controllerType))
            {
                lock (TypeCacheSyncRoot)
                {
                    if (!TypeCache.TryGetValue(controllerName, out controllerType))
                    {
                        controllerType = (Type.GetType(string.Format(ControlerNamespace1, controllerName, AppConfigManager.PartnerSetting.Namespace), false, true) ??
                                          Type.GetType(string.Format(ControlerNamespace2, controllerName), false, true) ??
                                         Type.GetType(string.Format(ControlerNamespace3, controllerName, AppConfigManager.PartnerSetting.Namespace), false, true) ??
                                         Type.GetType(string.Format(ControlerNamespace4, controllerName), false, true));
                        if (controllerType == null)
                        {
                            throw new HttpException(404, string.Format("Can't find controler [{0}], please make sure one of following class is correct:\r\n {1}\r\n {2}\r\n {3}\r\n {4}"
                                , controllerName
                                , string.Format(ControlerNamespace1, controllerName, AppConfigManager.PartnerSetting.Namespace)
                                , string.Format(ControlerNamespace2, controllerName)
                                , string.Format(ControlerNamespace3, controllerName, AppConfigManager.PartnerSetting.Namespace)
                                , string.Format(ControlerNamespace4, controllerName)
                            ));
                        }
                        TypeCache.Add(controllerName, controllerType);
                    }
                }
            }

            return CreateController(context, controllerType);
        }

        void IControllerFactory.ReleaseController(IController controller)
        {
            if (controller is IDisposable)
                ((IDisposable)controller).Dispose();
        }


        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }
    }
}