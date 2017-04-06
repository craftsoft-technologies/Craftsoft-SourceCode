using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace APP.Web.Common.Models
{
    [Serializable]
    public class DropdownList
    {
        [XmlAttribute("Type")]
        public string EntityType { get; set; }
        [XmlElement("Item")]
        public List<DropdownListItem> Items { get; set; }

        public DropdownList()
        {
            this.Items = new List<DropdownListItem>();
        }

        public DropdownList(string type)
            : this()
        {
            this.EntityType = type;
        }
    }

    [Serializable]
    public class DropdownListItem
    {
        [XmlAttribute]
        public string Text { get; set; }
        [XmlAttribute]
        public string Value { get; set; }

        public DropdownListItem()
        { }

        public DropdownListItem(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }
    }

    [Serializable]
    [XmlRoot("DropdownListEntities")]
    public class DropdownListCollections
    {
        [XmlElement("DropdownList")]
        public DropdownList[] Entities { get; set; }

        public DropdownListCollections()
        {
            this.Entities = new DropdownList[Enum.GetValues(typeof(DropdownListType)).Length];
        }

        public DropdownList this[int index]
        {
            get
            {
                return this.Entities[index];
            }
            set
            {
                this.Entities[index] = value;
            }
        }
    }

    public enum DropdownListType
    {
        Drop,
        TimeZone
    }
}
