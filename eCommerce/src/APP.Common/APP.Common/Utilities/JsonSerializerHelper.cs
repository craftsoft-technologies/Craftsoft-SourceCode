using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace APP.Common.Utilities
{
    public class JsonSerializerHelper
    {
        private static readonly Dictionary<string, DataContractJsonSerializer> cache = new Dictionary<string, DataContractJsonSerializer>();
        private static readonly object syncRoot = new object();

        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return "{}";
            }
            DataContractJsonSerializer serializer = GetSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static TReturn ToObj<TReturn>(string json)
        {
            DataContractJsonSerializer serializer = GetSerializer(typeof(TReturn));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (TReturn)serializer.ReadObject(stream);
            }
        }

        public static DataContractJsonSerializer GetSerializer(Type type)
        {
            DataContractJsonSerializer serializer;
            if (!cache.TryGetValue(type.Name, out serializer))
            {
                lock (syncRoot)
                {
                    if (!cache.TryGetValue(type.Name, out serializer))
                    {
                        serializer = new DataContractJsonSerializer(type);
                        cache[type.Name] = serializer;
                    }
                }
            }

            return serializer;
        }

    }
}
