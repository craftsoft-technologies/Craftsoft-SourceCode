using System;
using System.Xml.Serialization;

namespace APP.Web.eCommerce.BLL.Configuration
{
    [Serializable]
    public class RedirectSetting
    {
        public RedirectSetting(string beforeLoginFormat, string postLoingFormat)
        {
            this.BeforeLoginFormat = beforeLoginFormat;
            this.PostLoingFormat = postLoingFormat;
        }

        /// <summary>
        /// Format: {0} culture, {1} oneTimeToken, {2} token
        /// </summary>
        [XmlAttribute]
        public string BeforeLoginFormat { get; set; }

        /// <summary>
        /// Format: {0} culture, {1} oneTimeToken, {2} token
        /// </summary>
        [XmlAttribute]
        public string PostLoingFormat { get; set; }
    }
}
