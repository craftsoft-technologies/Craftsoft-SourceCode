using System;

namespace APP.Infrastructure.SsoManagement.Model
{
    class CurrentUserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime LastSucessfulLoginTime { get; set; }
        public DateTime LastActivityTime { get; set; }
    }
}
