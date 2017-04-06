using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Entities.Enum
{
    public class MemberEnum
    {
        public enum MemberStatus
        {
            NA = 0,
            Active = 1,
            InActive = 2,
            Suspended = 3
        }

        public enum accounttype
        {
            NA = 0,
            Real = 1,
            Test = 2
        }
        
        public enum Gender
        {
            NA = 0,
            Male = 1,
            Female = 2,
            Unknown = 3,
        }

        public enum MemberLockStatus
        {
            UnLocked = 0,
            Locked = 1
        }

        public enum MemberLoginStatus
        {
            Success = 1,
            Fail = 0
        }
        
        public enum ContactStatus
        {
            NotVerified = 0,
            Valid = 1,
            Invalid = 2
        }
        
        public enum OnlineStatus
        {
            NA = 0,
            Active = 1,
            Idle = 2
        }

        public enum Channel
        {
            NA = 0,
            Web = 1,
            Mobile = 3
        }

        public enum MemberLoginChannel
        {
            MemberSite = 1,
            MobileSite = 2,
            ExternalAPI = 3
        }

        public enum MemberSupportedChannel
        {
            Website = 1,
            Mobile = 2,
            App_Android = 3,
            App_IOS = 4
        }

        public enum RegisteredChannel
        {
            Website = 1,
            Mobile = 2
        }
    }
}
