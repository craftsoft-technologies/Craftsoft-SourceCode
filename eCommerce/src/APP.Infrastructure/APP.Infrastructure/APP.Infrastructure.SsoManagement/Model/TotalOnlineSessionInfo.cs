using System.Collections.Generic;

namespace APP.Infrastructure.SsoManagement.Model
{
    public class TotalOnlineSessionInfo
    {
        public int TotalOnlineUser
        {
            get
            {
                if (this.OnlineUser != null)
                {
                    return this.OnlineUser[0].TotalOnlineUserCount;
                }
                else
                {
                    return 0;
                }
            }
        }
        public List<SessionInfo> OnlineUser { get; set; }
    }
}
