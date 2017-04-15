using APP.Infrastructure.SsoManagement.Model;
using APP.Infrastructure.SsoManagement.Modules;

namespace APP.Infrastructure.SsoManagement.Policies
{
    public class NormalSignInPolicy : ISignInPolicy
    {
        #region ISignInPolicy Members

        public int WebsiteID
        {
            get;
            set;
        }

        public NormalSignInPolicy(int websiteId)
        {
            WebsiteID = websiteId;
        }

        public SessionInfo Login(SessionInfo session)
        {
            ISessionModule sessionModule = Common.SSOFactory.CreateSessionModule(WebsiteID);
            sessionModule.CreateSession(session);
            SessionInfo ret = sessionModule.GetSession(session.TokenID);
            return ret;
        }

        #endregion
    }
}
