using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.AppCode.API
{
    public class PartnerHttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private const string ControlerNamespace1 = "APP.Web.eCommerce.Website._Partners.{1}.API.{0}Controller";
        private const string ControlerNamespace2 = "APP.Web.eCommerce.Website.API.{0}Controller";

        #region V1

        //private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllers;

        //public PartnerHttpControllerSelector(HttpConfiguration configuration)
        //    : base(configuration)
        //{
        //    _configuration = configuration;
        //    _controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
        //}

        //public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        //{
        //    CultureInfo cultureEn = new CultureInfo(AppConfigManager.LanguageInfos[0].CultureName);
        //    Thread.CurrentThread.CurrentCulture = cultureEn;

        //    var controllerName = base.GetControllerName(request);
        //    if (!string.IsNullOrEmpty(controllerName))
        //    {
        //        HttpControllerDescriptor controllerDescriptor;
        //        if (_controllers.Value.TryGetValue(controllerName, out controllerDescriptor))
        //        {
        //            return controllerDescriptor;
        //        }
        //        else
        //        {
        //            return base.SelectController(request);
        //        }
        //    }
        //    else
        //    {
        //        throw new HttpException(404, string.Format("Can't find controler [{0}]", controllerName));
        //    }
        //}

        //public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        //{
        //    return _controllers.Value;
        //} 
        #endregion

        #region V2

        public PartnerHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
            LazyStorage.ContActions = new Lazy<ControllerActionCollection>(InitializeContActions);
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            CultureInfo cultureEn = new CultureInfo(AppConfigManager.LanguageInfos[0].CultureName);
            Thread.CurrentThread.CurrentCulture = cultureEn;

            Thread.CurrentThread.CurrentUICulture = cultureEn;

            var controllerName = base.GetControllerName(request);
            if (!string.IsNullOrEmpty(controllerName))
            {
                HttpControllerDescriptor controllerDescriptor;
                if (LazyStorage.ContActions.Value.Controllers.TryGetValue(controllerName, out controllerDescriptor))
                {
                    return controllerDescriptor;
                }
                else
                {
                    return base.SelectController(request);
                }
            }
            else
            {
                throw new HttpException(404, string.Format("Can't find controler [{0}]", controllerName));
            }
        }

        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return LazyStorage.ContActions.Value.Controllers;
        }
        #endregion

        #region Private Method
        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

            foreach (Type t in controllerTypes)
            {
                var segments = t.Namespace.Split(Type.Delimiter);

                var controllerName = t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);
                string currentPartner = string.Format(ControlerNamespace1, controllerName, AppConfigManager.PartnerSetting.Namespace);

                if (dictionary.Keys.Contains(controllerName))
                {
                    if (dictionary[controllerName].ControllerType.FullName != currentPartner && t.FullName == currentPartner)
                    {
                        dictionary[controllerName] = new HttpControllerDescriptor(_configuration, t.Name, t);
                    }
                }
                else
                {
                    dictionary[controllerName] = new HttpControllerDescriptor(_configuration, t.Name, t);
                }
            }

            return dictionary;
        }

        private Dictionary<string, Dictionary<string, ReflectedHttpActionDescriptor>> InitializeActionDictionary(List<HttpControllerDescriptor> controllers)
        {
            Dictionary<string, Dictionary<string, ReflectedHttpActionDescriptor>> actions = new Dictionary<string, Dictionary<string, ReflectedHttpActionDescriptor>>();

            foreach (var controller in controllers)
            {
                var allMethods = controller.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                var validMethods = Array.FindAll(allMethods, IsValidActionMethod);
                Dictionary<string, ReflectedHttpActionDescriptor> actionDescriptors = new Dictionary<string, ReflectedHttpActionDescriptor>();

                foreach (var actionDescriptor in validMethods.Select(m => new ReflectedHttpActionDescriptor(controller, m)))
                {
                    string key = string.Empty;
                    System.Web.Http.RouteAttribute route = actionDescriptor.MethodInfo.GetCustomAttribute<System.Web.Http.RouteAttribute>();
                    key = route != null ? route.Template : actionDescriptor.ActionName;
                    int paramIdentity = actionDescriptors.Keys.Count(s => s.Contains(key + "-"));
                    key = string.Format("{0}-{1}", key.ToLowerInvariant(), paramIdentity);

                    if (actionDescriptors.Keys.Contains(key))
                    {
                        if (actionDescriptor.MethodInfo.DeclaringType == controller.ControllerType && actionDescriptor.MethodInfo.ReflectedType == controller.ControllerType)
                        {
                            actionDescriptors[key] = actionDescriptor;
                        }
                    }
                    else
                    {
                        actionDescriptors.Add(key, actionDescriptor);
                    }
                }

                actions.Add(controller.ControllerName, actionDescriptors);
            }

            return actions;
        }

        private ControllerActionCollection InitializeContActions()
        {
            ControllerActionCollection conAction = new ControllerActionCollection();
            conAction.Controllers = InitializeControllerDictionary();
            conAction.Actions = InitializeActionDictionary(conAction.Controllers.Values.ToList());
            return conAction;
        }

        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsSpecialName) return false;
            return !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(ApiController));
        }
        #endregion
    }
}