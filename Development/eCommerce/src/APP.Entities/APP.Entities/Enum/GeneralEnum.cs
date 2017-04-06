using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Entities.Enum
{
    public class GeneralEnum
    {
        public enum ModuleType
        {
            NA = 0,
            Member = 1,
            MemberAccount = 2,
            MemberCommunication = 3,
            Adjustment = 4,
            Payment = 5,
            Currency = 6,
            User = 7,
            Role = 8,
            WhiteList = 9,
        }

        public enum GeneralSetting
        {
            IsAboveThreshold = 1,
        }

        public enum PartnerCode
        {
            Noise = 0
        }

        public enum CDNSettingCode
        {
            Themes = 1
        }
    }
}
