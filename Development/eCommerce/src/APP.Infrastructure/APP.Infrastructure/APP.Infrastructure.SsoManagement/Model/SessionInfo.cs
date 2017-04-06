using System;

namespace APP.Infrastructure.SsoManagement.Model
{
    public class SessionInfo
    {
        public int SsoOnlineUserID { get; set; }
        public int WebsiteID { get; set; }
        public int AppID { get; set; }
        public string TokenID { get; set; }
        public string LoginName { get; set; }
        public int UserID { get; set; }
        public string IPAddress { get; set; }
        public string Remarks { get; set; }
        public bool IsLoginExpired { get; set; }
        public int LoginAttempt { get; set; }

        public int TotalOnlineUserCount { get; set; }

        public DateTime LastActivitiesTime { get; set; }
        public DateTime LastSuccessfulLogin { get; set; }
        public DateTime LastFailedLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public int Channel { get; set; }
        public string ServerIP { get; set; }
        public int MemberSiteActivityID { get; set; }
    }
}
