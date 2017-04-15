using System;
using System.Linq;
using System.Threading;
using APP.Entities.Entities;
using APP.Infrastructure.SsoManagement.Model;
using APP.Web.Common.AppCache;
using APP.Web.Common.Utilities;
using APP.Web.Common.Models;
using APP.Web.eCommerce.BLL.Configuration;

namespace APP.Web.eCommerce.BLL.Model
{
    [Serializable]
    public class SessionData
    {
        public string Ip { get; set; }
        public string CountryCode { get; set; }
        public LanguageInfo CurrentLanguageInfo { get; set; }

        public bool HasLogin
        {
            get
            {
                return SessionInfo != null;
            }
        }

        public bool HasCheckIpBlock { get; set; }
        public bool IsIpBlocked { get; set; }

        public bool PopUpRegistrationPage { get; set; }

        public SessionInfo SessionInfo { get; set; }

        public MemberEntity MemberEntity { get; set; }

        public ClientData ClientData { get; set; }

        public void RefreshClientData()
        {
            ClientData = new ClientData();
            if (HasLogin)
            {
                var utcOffset = DateUtil.GetUtcOffset(MemberEntity.timeZone);
                ClientData.TimeZoneDisplayName = DateUtil.GetTimeZoneDisplayName(MemberEntity.timeZone);
                ClientData.UtcOffset = (int)utcOffset;
                ClientData.Login = new Postlogin
                {
                    MemberName = MemberEntity.memberName,//login name
                    GivenName = MemberEntity.givenName,//sure name
                    LastName = MemberEntity.lastName,//family name
                    LastSuccessLogin = MemberEntity.memberLogin.lastSuccessLogin.ToClientDateTime(utcOffset),
                    PreferedLanguage = MemberEntity.preferedLanguage
                };
            }
            else
            {
                ClientData.TimeZoneDisplayName = AppConfigManager.SystemSetting.DefaultTimeZone;
                ClientData.UtcOffset = DateUtil.GetUtcOffset(ClientData.TimeZoneDisplayName);
                ClientData.Login = null;
                ClientData.PopupImportantAnnouncement = false;
            }
        }

        public SessionData(string ip, string countryCode)
        {
            RefreshClientData();
            Ip = ip;
            CountryCode = countryCode;
        }
    }
}
