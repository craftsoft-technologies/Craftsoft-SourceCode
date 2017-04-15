using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using APP.Common.Exception;
using APP.Common.Utilities;
using APP.Infrastructure.SsoManagement.Common;
using APP.Web.Common.Mvc.ActionResults;
using APP.Web.Common.Properties;
using APP.Web.Common.Utilities;
using APP.Web.eCommerce.BLL;
using APP.Web.eCommerce.BLL.Configuration;
using APP.Web.eCommerce.BLL.Model;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class MultiCultureControllerBase : Controller
    {
        public SessionData SessionData { get; set; }

        public LanguageInfo LanguageInfo { get; set; }

        protected IActionInvoker CreateActionInvoker(bool checkIp)
        {
            var sessionData = Session[SessionManager.SessionDataKey] as SessionData;
            if (sessionData == null) //If session doesn't exist, return default SessionData
            {
                var currentHost = this.HttpContext.Request.Url.Host.ToLower();
                foreach (var hostRedirect in AppConfigManager.SiteHostRedirect)
                {
                    if (currentHost == hostRedirect.FromHost.ToLower())
                    {
                        var newUrl = this.HttpContext.Request.Url.OriginalString.Replace("//" + hostRedirect.FromHost, "//" + hostRedirect.ToHost);
                        this.HttpContext.Response.Redirect(newUrl);
                        return null;
                    }
                }

                string ip = CommonHelper.GetClientIP(System.Web.HttpContext.Current.Request);
                string countryCode = ip; //!= "::1" ? SecurityManager.GetCountryCodeByIp(ip) : "ID"; // default to Malaysia

                LogHelper.User.InfoFormat(" Created Sesssion {0},{1}", ip, countryCode);
                sessionData = new SessionData(ip, countryCode);
                Session[SessionManager.SessionDataKey] = sessionData;
            }
            sessionData.CurrentLanguageInfo = LanguageInfo;
            this.SessionData = sessionData;//Set sessionData to Controller
            ViewBag.SessionData = SessionData;//Set sessionData to ViewBag
            ViewBag.LanguageInfo = LanguageInfo;
            ViewBag.PartnerCode = AppConfigManager.PartnerFolderName;
            return base.CreateActionInvoker();
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            return CreateActionInvoker(true);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ModelState.IsValid)
            {
                if (SessionData.HasLogin)
                {
                    System.Threading.Thread.SetData(System.Threading.Thread.GetNamedDataSlot("LoginName"), SessionData.MemberEntity.memberLogin.loginName);
                }
                base.OnActionExecuting(filterContext);
            }
            else
            {
                var messageBuilder = new StringBuilder();
                for (int i = 0; i < ModelState.Keys.Count; i++)
                {
                    ModelState valueState = ModelState.Values.ElementAt(i);
                    if (valueState.Errors.Count > 0)
                    {
                        string key = ModelState.Keys.ElementAt(i);
                        if (key == "AmountNum")
                        {
                            foreach (ModelError error in valueState.Errors)
                            {
                                messageBuilder.AppendLine(string.Format(error.ErrorMessage));
                            }
                        }
                        else
                        {
                            messageBuilder.AppendLine(string.Format("[{0}]:{1}"
                            , key
                            , valueState.Value == null ? "" : valueState.Value.AttemptedValue));
                            foreach (ModelError error in valueState.Errors)
                            {
                                messageBuilder.AppendLine(string.Format(" {0},{1}", error.ErrorMessage, error.Exception));
                            }
                        }
                    }
                }

                LogHelper.ExceptionLog.InfoFormat(messageBuilder.ToString());
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new ExceptionModel { ErrorMessage = messageBuilder.ToString() });
                }
                else
                {
                    filterContext.Result = new ContentResult { Content = messageBuilder.ToString() };
                }
            }
        }

        protected override void Execute(RequestContext requestContext)
        {
            try
            {
                base.Execute(requestContext);
            }

            catch (BaseException ex)
            {
                string localizedErrorMessage = string.Empty;

                if (ex.ErrorCode == "DATA_DUPLICATED" && ex.Message.Contains("PK_Website_LoginName"))
                {
                    localizedErrorMessage = ErrorMessageUtility.GetLocalizedErrorMessage("DATA_DUPLICATED_LOGINNAME");
                }
                else
                {
                    localizedErrorMessage = ErrorMessageUtility.GetLocalizedErrorMessage(ex.ErrorCode);
                }

                if (string.IsNullOrEmpty(localizedErrorMessage))
                {
                    localizedErrorMessage = ex.ErrorCode;
                }
                LogHelper.Server.Error(localizedErrorMessage);
                HandleException(requestContext, ex, localizedErrorMessage);
            }
            catch (HttpException ex)
            {
                LogHelper.Exception(ex);
                throw; //Config the exception handling in web.config
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex);
                HandleException(requestContext, ex, ex.Message);
            }
        }

        private void HandleException(RequestContext requestContext, Exception ex, string displayErrorMessage)
        {
            if (requestContext.HttpContext.Request.IsAjaxRequest())
            {
                HttpResponseBase response = requestContext.HttpContext.Response;
                string errJson = JsonSerializerHelper.Serialize(new ExceptionModel { ErrorMessage = displayErrorMessage });
                response.AppendHeader("Content-type", "application/json");
                response.Write(errJson);
                //response.StatusCode = 500;
                response.End();
            }
            else
            {
                Response.Write(displayErrorMessage);
            }
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            ViewData["SessionData"] = SessionData;
            return base.View(viewName, masterName, model);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDataContractResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected ActionResult SessionLost(bool isAjaxRequest)
        {
            if (isAjaxRequest)
            {
                return Json(new ExceptionModel { SessionLost = true, ErrorMessage = "" });//ErrorMessage.MEMBER_SESSION_LOST });
            }
            return RedirectToAction("membersessionlost", "home");
        }

        protected ActionResult ResetPasswordKickoutAndClearSession(bool isAjaxRequest)
        {
            Session.Clear();
            if (isAjaxRequest)
            {
                return Json(new ExceptionModel { SessionLost = true, ErrorMessage = "" }); //ErrorMessage.MEMBER_RESETPASSWORD_KICKOUT });
            }
            return RedirectToAction("memberresetkickout", "home");
        }

        protected ActionResult KickoutAndClearSession(bool isAjaxRequest)
        {
            Session.Clear();
            if (isAjaxRequest)
            {
                return Json(new ExceptionModel { SessionLost = true, ErrorMessage = "" }); //ErrorMessage.MEMBER_SESSION_Kickout });
            }
            return RedirectToAction("memberkickout", "home");
        }

        protected ActionResult KickoutAndClearSessionByBO(bool isAjaxRequest)
        {
            Session.Clear();
            if (isAjaxRequest)
            {
                return Json(new ExceptionModel { SessionLost = true, ErrorMessage = "" }); //ErrorMessage.MEMBER_SESSION_Kickout_ByBO });
            }
            return RedirectToAction("memberkickout", "home");
        }

        protected void CheckSessionExists()
        {
            if (SessionManager.Current.SessionInfo != null)
            {
                throw new Exception("");//ErrorMessage.MEMBER_SESSION_EXISTS);
            }
        }

        #region Nested type: ExceptionModel

        [DataContract]
        public class ExceptionModel
        {
            [DataMember(Name = "_err", EmitDefaultValue = true)]
            public string ErrorMessage { get; set; }

            [DataMember(Name = "_sl", EmitDefaultValue = false)]
            public bool? SessionLost { get; set; }
        }

        #endregion
    }
}