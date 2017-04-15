using System;
using System.Globalization;
using System.Xml.Serialization;

namespace APP.Web.eCommerce.BLL.Configuration
{
    [Serializable]
    public class LanguageInfo
    {
        private string cultureName;

        [XmlAttribute]
        public string CultureName
        {
            get { return cultureName; }
            set
            {
                Culture = new CultureInfo(value);
                cultureName = value;
            }
        }

        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        [XmlIgnore]
        public CultureInfo Culture { get; set; }
    }
}
