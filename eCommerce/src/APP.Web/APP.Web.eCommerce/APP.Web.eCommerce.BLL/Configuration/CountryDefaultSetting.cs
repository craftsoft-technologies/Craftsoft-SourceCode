using System;
using System.Xml.Serialization;

namespace APP.Web.eCommerce.BLL.Configuration
{
    [Serializable]
    public class CountryDefaultSetting
    {
        [XmlAttribute]
        public string CountryCode { get; set; }

        [XmlAttribute]
        public string DefaultLanguage { get; set; }

        [XmlAttribute]
        public string DefaultCurrency { get; set; }

        [XmlAttribute]
        public string DefaultTimeZone { get; set; }
    }
}
