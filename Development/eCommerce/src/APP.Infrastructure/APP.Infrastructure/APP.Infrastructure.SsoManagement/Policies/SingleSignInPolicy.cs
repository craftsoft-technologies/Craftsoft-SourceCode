using APP.Infrastructure.SsoManagement.Model;
using APP.Infrastructure.SsoManagement.Modules;

namespace APP.Infrastructure.SsoManagement.Policies
{
    public class SingleSignInPolicy : ISignInPolicy
    {
        #region ISignInPolicy Members

        public int WebsiteID
        {
            get;
            set;
        }

        public SingleSignInPolicy(int websiteId)
        {
            WebsiteID = websiteId;
        }

        public SessionInfo Login(SessionInfo session)
        {
            ISessionModule sessionModule = Common.SSOFactory.CreateSessionModule(WebsiteID);

            //set previous session IsLoginExpired to true to kick out previous session
            sessionModule.InActivateSessionByWebsiteIdAndUserId(WebsiteID, session.UserID);

            sessionModule.CreateSession(session);
            SessionInfo ret = sessionModule.GetSession(session.TokenID);
            return ret;
        }

        #endregion
    }
}
