using System;
using System.Web;
using APP.Infrastructure.SsoManagement.Common;
using APP.Infrastructure.SsoManagement.DAL;
using APP.Infrastructure.SsoManagement.Model;

namespace APP.Infrastructure.SsoManagement.Modules
{
    public class SessionModule : ISessionModule
    {
        #region ISessionModule Members

        public int WebsiteID { get; set; }

        public SessionInfo GetSession(string tokenID)
        {
            SessionInfo SessionInfo = SessionModuleDAL.GetSession(tokenID);
            return SessionInfo;
        }

        public SessionInfo GetSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            SessionInfo SessionInfo = SessionModuleDAL.GetSessionByWebsiteIdAndUserId(websiteId, userId);
            return SessionInfo;
        }

        public string CreateSession(SessionInfo session)
        {
            session.WebsiteID = WebsiteID;
            session.TokenID = Guid.NewGuid().ToString("N");
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                session.IPAddress = CommonHelper.GetClientIP(HttpContext.Current.Request);
            }

            SessionModuleDAL.CreateSession(session);
            return session.TokenID;
        }

        public void RemoveSessionByTokenID(string tokenId)
        {
            SessionModuleDAL.RemoveSessionByTokenID(tokenId);
        }

        public bool CheckSession(string tokenID)
        {
            return SessionModuleDAL.CheckSession(tokenID);
        }

        public void InActivateSessionByTokenID(string tokenid)
        {
            SessionModuleDAL.InActivateSessionByTokenID(tokenid);
        }

        public void InActivateSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            SessionModuleDAL.InActivateSessionByWebsiteIdAndUserId(websiteId, userId);
        }

        public void InActivateSessionByWebsiteIdAndLoginName(int websiteId, string loginName)
        {
            SessionModuleDAL.InActivateSessionByWebsiteIdAndLoginName(websiteId, loginName);
        }

        public void RemoveSessionByWebsiteIdAndUserId(int websiteId, int userId)
        {
            SessionModuleDAL.RemoveSessionByWebsiteIdAndUserId(websiteId, userId);
        }

        public TotalOnlineSessionInfo GetTotalOnlineUser(int websiteId)
        {
            TotalOnlineSessionInfo ret = new TotalOnlineSessionInfo();
            ret.OnlineUser = SessionModuleDAL.GetWebSiteTotalOnlineUser(websiteId);
            return ret;
        }

        public void RegisterSuccessfulLoginTime(string loginID)
        {
            SessionModuleDAL.RegisterSuccessfulLoginTime(loginID, WebsiteID);
        }

        public void RegisterFailedLoginTime(string loginID)
        {
            SessionModuleDAL.RegisterFailedLoginTime(loginID, WebsiteID);
        }

        public DateTime[] GetLoginTime(string loginID)
        {
            return SessionModuleDAL.GetLoginTime(loginID, WebsiteID);
        }

        public int UpdateMemberActivity(string loginName, int memberSiteActivityID)
        {
            return SessionModuleDAL.UpdateMemberActivity(loginName, memberSiteActivityID);
        }

        public void KickoutMemberOnline(string tokenid)
        {
            SessionModuleDAL.KickoutMemberOnline(tokenid);
        }
        #endregion
    }
}
