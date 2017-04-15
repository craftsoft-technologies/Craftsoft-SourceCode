using System;

namespace APP.Entities.Entities
{
    [Serializable]
    public class MemberLoginEntity
    {
        public int memberId { get; set; }
        public string loginName { get; set; }
        public bool isForceChangePassword { get; set; }
        public bool isUserLock { get; set; }
        public int loginFailAttempt { get; set; }
        public DateTime lastSuccessLogin { get; set; }
        public DateTime lastFailLogin { get; set; }
        public string Password { get; set; }

    }
}
