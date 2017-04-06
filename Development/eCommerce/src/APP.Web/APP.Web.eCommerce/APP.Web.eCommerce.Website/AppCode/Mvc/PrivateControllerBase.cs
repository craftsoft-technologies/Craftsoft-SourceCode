using System;
using System.Web.Mvc;
using APP.Infrastructure.SsoManagement.Common;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.Website.Mvc
{
    public class PrivateControllerBase : MultiCultureControllerBase
    {
        /// <summary>
        /// GetSessionStatus, have cache
        /// </summary>
        /// <returns></returns>
        SessionStatus GetSessionStatus()
        {
            var nextSSoCheckTime = Session["nextSSoCheckTime"];

            if (nextSSoCheckTime == null || (DateTime)nextSSoCheckTime < DateTime.Now)
            {
                var status = SessionStatus.NA;//UserManager.GetSessionStatus(SessionData.SessionInfo.TokenID);
                Session["nextSSoCheckTime"] = DateTime.Now.AddSeconds(AppConfigManager.SystemSetting.SsoCheckIntervalActivity);

                if ((DateTime.Now - SessionData.SessionInfo.LastActivitiesTime).Minutes >= AppConfigManager.SystemSetting.SsoCheckIntervalActivity)
                {
                    int result = 0;//UserManager.UpdateMemberActivity(SessionData.SessionInfo.LoginName, SessionData.SessionInfo.MemberSiteActivityID);
                    if (result > 0)
                    {
                        SessionData.SessionInfo.LastActivitiesTime = DateTime.Now;
                    }
                }

                return status;
            }

            return SessionStatus.Valid;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!SessionData.HasLogin)
            {
                filterContext.Result = SessionLost(filterContext.HttpContext.Request.IsAjaxRequest());
            }
            else
            {
                var status = GetSessionStatus();
                switch (status)
                {
                    case SessionStatus.Valid:
                        //Thread.SetData(SecurityManager.AuditLogDataStoreSlot, SessionData.MemberEntity.memberLogin.loginName);
                        break;

                    case SessionStatus.Expired:
                        filterContext.Result = KickoutAndClearSession(filterContext.HttpContext.Request.IsAjaxRequest());
                        break;
                    case SessionStatus.Kickout:
                        filterContext.Result = KickoutAndClearSessionByBO(filterContext.HttpContext.Request.IsAjaxRequest());
                        break;
                    case SessionStatus.NotExists://NotExists euqal to PasswordReset (Provided by Wilson)
                    default:
                        filterContext.Result = ResetPasswordKickoutAndClearSession(filterContext.HttpContext.Request.IsAjaxRequest());
                        break;
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
}