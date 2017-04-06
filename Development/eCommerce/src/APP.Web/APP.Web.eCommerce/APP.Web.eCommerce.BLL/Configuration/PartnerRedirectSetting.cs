using System;
using System.Xml.Serialization;

namespace APP.Web.eCommerce.BLL.Configuration
{
    [Serializable]
    public class PartnerRedirect
    {
        [XmlAttribute]
        public string FromHost { get; set; }

        [XmlAttribute]
        public string ToHost { get; set; }
    }
}
