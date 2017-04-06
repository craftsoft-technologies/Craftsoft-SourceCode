using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace APP.Common.Utilities
{
    public static class DataContractSerializerHelper
    {
        private static readonly Dictionary<string, DataContractSerializer> serializerPool = new Dictionary<string, DataContractSerializer>();
        private static readonly object syncRoot = new object();

        public static string ToXml<T>(T obj) where T : class
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var backing = new System.IO.StringWriter())
            using (var writer = new System.Xml.XmlTextWriter(backing))
            {
                serializer.WriteObject(writer, obj);
                return backing.ToString();
            }
        }

        public static TReturn ToObj<TReturn>(string xml)
        {
            var serializer = new DataContractSerializer(typeof(TReturn));
            using (var backing = new System.IO.StringReader(xml))
            using (var reader = new System.Xml.XmlTextReader(backing))
            {
                return (TReturn)serializer.ReadObject(reader);
            }
        }

        private static DataContractSerializer GetXmlSerializer(Type type)
        {
            DataContractSerializer serializer;
            if (!serializerPool.TryGetValue(type.FullName, out serializer))
            {
                lock (syncRoot)
                {
                    if (!serializerPool.TryGetValue(type.FullName, out serializer))
                    {
                        serializer = new DataContractSerializer(type);
                        serializerPool.Add(type.FullName, serializer);
                    }
                }
            }

            return serializer;
        }
    }
}
