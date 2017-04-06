using System;
using System.Xml.Serialization;

namespace APP.Web.eCommerce.BLL.Configuration
{
    [Serializable]
    public class DomainInfo
    {
        [XmlAttribute]
        public string Domain { get; set; }

        [XmlAttribute]
        public string Key { get; set; }
    }
}
