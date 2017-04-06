using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace APP.Common.Utilities
{
    public static class XmlSerializerHelper
    {
        private static readonly Dictionary<string, XmlSerializer> serializerCache = new Dictionary<string, XmlSerializer>();
        private static readonly object syncRoot = new object();

        public static string ToXml<T>(T obj)
            where T : class
        {
            if (obj == null)
            {
                return string.Empty;
            }
            XmlSerializer serializer = GetXmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static string ToXmlWithoutNamespace<T>(T obj)
            where T : class
        {
            if (obj == null)
            {
                return string.Empty;
            }
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            XmlSerializer serializer = GetXmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj, ns);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static T ToObj<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }
            XmlSerializer serializer = GetXmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        public static T ToObjFromFile<T>(string xmlFile)
        {
            return ToObj<T>(File.ReadAllText(xmlFile));
        }

        private static XmlSerializer GetXmlSerializer(Type type)
        {
            XmlSerializer serializer;
            if (!serializerCache.TryGetValue(type.FullName, out serializer))
            {
                lock (syncRoot)
                {
                    if (!serializerCache.TryGetValue(type.FullName, out serializer))
                    {
                        serializer = new XmlSerializer(type);
                        serializerCache.Add(type.FullName, serializer);
                    }
                }
            }

            return serializer;
        }

        public static string SerializeXml(Type type, object obj, bool clearAttributes, bool wellFormatting)
        {
            string data_string = null;
            using (XmlTextWriter writer = new XmlTextWriter(new MemoryStream(), Encoding.UTF8))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                serializer.Serialize(writer, obj);
                writer.BaseStream.Position = 0;
                using (StreamReader sr = new StreamReader(writer.BaseStream, Encoding.UTF8))
                    data_string = sr.ReadToEnd();
            }
            if (clearAttributes || wellFormatting)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data_string);

                if (clearAttributes) ClearAttributesFromElement(xml.DocumentElement);

                using (XmlTextWriter writer = new XmlTextWriter(new MemoryStream(), Encoding.UTF8))
                {
                    if (wellFormatting)
                    {
                        writer.Formatting = Formatting.Indented;
                        writer.Indentation = 4;
                        writer.IndentChar = ' ';
                    }
                    xml.WriteContentTo(writer);
                    writer.Flush();
                    writer.BaseStream.Position = 0;
                    using (StreamReader sr = new StreamReader(writer.BaseStream, Encoding.UTF8))
                        data_string = sr.ReadToEnd();
                }
            }

            return data_string;
        }

        public static string SerializeXml<T>(T obj, bool clearAttributes, bool wellFormatting)
        {
            return SerializeXml(typeof(T), obj, clearAttributes, wellFormatting);
        }

        private static void ClearAttributesFromElement(XmlElement elem)
        {
            elem.RemoveAllAttributes();
            foreach (XmlNode node in elem.ChildNodes)
                if (node is XmlElement)
                    ClearAttributesFromElement((XmlElement)node);
        }

        public static T DeserializeXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlTextReader xr = new XmlTextReader(new StringReader(xml));
            T obj = (T)serializer.Deserialize(xr);

            return obj;
        }
    }
}
