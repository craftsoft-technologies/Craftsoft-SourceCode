using System;
using System.Xml.Serialization;
using APP.Entities.VO;


namespace APP.Web.eCommerce.BLL.Configuration
{
    public class SystemSetting
    {
        #region Properties

        public string Cdn { get; set; }
        public int CdnVersion { get; set; }
        public int CdnVers { get; set; }
        public string DefaultCountryCode { get; set; }
        public string LiveChatUrl { get; set; }
        public bool LiveChatShowStaticPage { get; set; }
        public string DefaultTimeZone { get; set; }
        public int SsoCheckIntervalActivity { get; set; }
        public string MailgunURI { get; set; }
        public string MailgunPublicKey { get; set; }
        public string UserCodeReceive { get; set; }
        public string HtmlContentManagerModule { get; set; }
        public string BMTServer { get; set; }
        public bool IsAPIEncryptionActivated { get; set; }
        #endregion
    }

    public class PartnerFolderSetting
    {
        public string PartnerFolderName { get; set; }
    }
}
