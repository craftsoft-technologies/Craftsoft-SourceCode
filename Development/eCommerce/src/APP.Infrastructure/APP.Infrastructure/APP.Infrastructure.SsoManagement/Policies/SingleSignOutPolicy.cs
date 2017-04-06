using APP.Infrastructure.SsoManagement.Model;
using APP.Infrastructure.SsoManagement.Modules;

namespace APP.Infrastructure.SsoManagement.Policies
{
    public class SingleSignOutPolicy : ISignOutPolicy
    {
        #region ISignOutPolicy Members
        public int WebsiteID { get; set; }

        public SingleSignOutPolicy(int websiteId)
        {
            WebsiteID = websiteId;
        }

        public void Logout(string TokenID)
        {
            ISessionModule sessionModule = Common.SSOFactory.CreateSessionModule(WebsiteID);
            SessionInfo sessionInfo = sessionModule.GetSession(TokenID);
            if (sessionInfo != null)
            {
                sessionModule.InActivateSessionByTokenID(TokenID);
            }
        }

        #endregion
    }
}
