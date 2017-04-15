using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Web.Common.Validation
{
    public class RegularPatterns
    {
        public const string LoginName = @"[a-zA-Z0-9]*";
        public const string PostalCode = @"[a-zA-Z0-9]*";
        public const string ContactNumber = @"[0-9]*";
        public const string ContactNumber1 = @"[0-9\+()\s]*";
        public const string AccountNumber = @"[a-zA-Z0-9\-]*";
        public const string HourMinuteSecond = "([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]";
        public const string HourMinute = "([01][0-9]|2[0-3]):[0-5][0-9]";
        public const string FieldTwoDecimal = @"\d+(\.\d{1,2})?";
        public const string Url = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
        public const string EmailInvalid = @"^((?!invalid).)*$";
    }
}
