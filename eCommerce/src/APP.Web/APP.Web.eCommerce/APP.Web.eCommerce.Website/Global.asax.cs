using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using DataAnnotationsExtensions.ClientValidation;
using APP.Common.Utilities;
using APP.Web.Common.Utilities;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Web.eCommerce.BLL.Model;
using APP.Web.eCommerce.Website.Mvc;
using APP.Web.eCommerce.Website.App_Start;

namespace APP.Web.eCommerce.Website
{
    public class Global : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
#if DEBUG
            ErrorMessageUtility.ValidResources();
            Common.Validation.ResourceKey.ValidResources();
#endif

            try
            {
                GlobalHelper.ServerStartUpTime = DateTime.Now;
                AppConfigManager.PartnerFolderName = XmlSerializerHelper.ToObj<PartnerFolderSetting>(File.ReadAllText(HttpContext.Current.Server.MapPath("\\") + "PartnerFolderSetting.config")).PartnerFolderName;
                string partnerConfigurationFolder = HttpContext.Current.Server.MapPath("\\_Partners\\" + AppConfigManager.PartnerFolderName + "\\Configuration\\");
                string baseConfigurationFolder = HttpContext.Current.Server.MapPath("\\Configuration\\");
                log4net.Config.XmlConfigurator.Configure(new FileInfo(GlobalHelper.GetFirstExistFileName(partnerConfigurationFolder, baseConfigurationFolder, "log4net.config")));
                LogHelper.Server.InfoFormat("Application_Start [{0}]", AppConfigManager.PartnerFolderName);

                LogHelper.Server.Info("1.Initialize Configuration:");
                GlobalHelper.InitializeConfiguration(partnerConfigurationFolder, baseConfigurationFolder);

                LogHelper.Server.Info("2.Initialize Localization framework (Merge Resource Files)");
                PartnerLocalizationManager.Initialize(AppConfigManager.PartnerSetting.Namespace);

                LogHelper.Server.Info("3.Register Routes");
                //GlobalConfiguration.Configuration(WebApiConfig.Register);
                GlobalHelper.RegisterRoutes(RouteTable.Routes);
                MVCRouteBlockerConfig.Configure(RouteTable.Routes);

                LogHelper.Server.Info("4.Initialize AppCacheManager");

                LogHelper.Server.Info("5.Initialize ViewEngines");
                GlobalHelper.InitViewEngine(AppConfigManager.PartnerSetting.Namespace);

                LogHelper.Server.Info("6.Initialize ControllerFactory");
                ControllerBuilder.Current.SetControllerFactory(typeof(LanguageControllerFactory));
                //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new PartnerHttpControllerSelector(GlobalConfiguration.Configuration));
                //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpActionSelector), new PartnerHttpActionSelector());

                LogHelper.Server.Info("7.Register DataAnnotation Validation Extensions");
                DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();

                LogHelper.Server.Info("8.Load favicon.ico");
                FaviconHttpHandler.Initialize(HttpContext.Current.Server.MapPath("\\_Partners\\" + AppConfigManager.PartnerFolderName + "\\favicon.ico"), HttpContext.Current.Server.MapPath("\\favicon.ico"));
            }
            catch (Exception ex)
            {
                LogHelper.Server.Error(ex);
                LogHelper.Exception(ex);
                throw;
            }
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            GlobalHelper.SessionCount += 1;
        }

        static object performanceLogKey = new object();

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.Context.Items.Add(performanceLogKey, DateTime.Now);
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            switch (base.Response.ContentType.Substring(Response.ContentType.Length - 4))
            {
                case "json"://End with json
                case "html"://End with html
                    var t = this.Context.Items[performanceLogKey];
                    if (t != null)
                    {
                        var cost = DateTime.Now - (DateTime)t;
                        LogHelper.RequestPerformance(cost, "[Request]:" + this.Request.Url.PathAndQuery);
                    }
                    break;

                default://css,js,image,jpeg,png
                    break;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                LogHelper.Server.Error(lastError);
            }
            else
            {
                LogHelper.Server.Error("Application_Error, but lastError is null");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            GlobalHelper.SessionCount -= 1;
            var sessionData = Session[SessionManager.SessionDataKey] as SessionData;
            if (sessionData != null && sessionData.SessionInfo != null)
            {
                //UserManager.Logout(sessionData.SessionInfo.LoginName, sessionData.SessionInfo.TokenID);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //LogHelper.Server.Info("[Server] STAR Application_End");
            var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime",
                                                                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                                                                        null, null, null);

            if (runtime == null)
                return;

            var shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                                                                             BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                                                                             null,
                                                                             runtime,
                                                                             null);

            var shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                                                                           BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                                                                           null,
                                                                           runtime,
                                                                           null);
            LogHelper.Server.Info("[Server]ShutDown Message: " + shutDownMessage);
            LogHelper.Server.Info("[Server]ShutDown Stack: " + shutDownStack + "\r\n\r\n");
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            switch (custom)
            {
                case "pathandquery":
                    return Request.Url.PathAndQuery;
                case "path":
                    return Request.Url.AbsolutePath;

                default:
                    throw new NotSupportedException();
            }
        }
    }

    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue,
                    CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}
