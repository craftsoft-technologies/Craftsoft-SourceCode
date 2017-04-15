using System;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;
using APP.Web.Common.Utilities;

namespace APP.Web.eCommerce.BLL.Model
{
    [Serializable]
    [DataContract]
    public class ClientData
    {
        public int UtcOffset { get; set; }

        [DataMember(Name = "t")]
        public string TimeForJs
        {
            get
            {
                CultureInfo cultureNow = Thread.CurrentThread.CurrentCulture;
                CultureInfo cultureEn = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = cultureEn;
                string date = DateTime.Now.ToClientDateTime(UtcOffset).ToString("yyyy,MM-1,dd,HH,mm,ss");//MM-1 is for JavaScript parse
                Thread.CurrentThread.CurrentCulture = cultureNow;

                return date;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        [DataMember(Name = "tz")]
        public string TimeZoneDisplayName { get; set; }

        [DataMember(Name = "l", EmitDefaultValue = false)]
        public Postlogin Login { get; set; }

        [DataMember(Name = "rl")]
        public string ReferralLink { get; set; }

        [DataMember(Name = "IA")]
        public bool PopupImportantAnnouncement { get; set; }

    }
}
