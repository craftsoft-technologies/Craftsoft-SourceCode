using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace APP.Web.eCommerce.BLL.Configuration
{
    [Serializable]
    public class MvcLinkInfo
    {
        [XmlAttribute]
        public string Controller { get; set; }
        [XmlAttribute]
        public string Action { get; set; }
        [XmlAttribute]
        public string ResourceKey { get; set; }
        public List<MvcLinkInfo> Children { get; set; }
        [XmlAttribute]
        public LinkName HightLightLink { get; set; }

        public MvcLinkInfo()
        {
            Children = new List<MvcLinkInfo>();
        }

        public MvcLinkInfo(LinkName hightLightLink, string controller, string action, string resourceKey)
            : this()
        {
            HightLightLink = hightLightLink;
            Controller = controller;
            Action = action;
            ResourceKey = resourceKey;
        }
    }
}
