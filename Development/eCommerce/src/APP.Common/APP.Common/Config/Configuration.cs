using System.IO;
using APP.Common.Utilities;
using System;

namespace APP.Common.Config
{
    public abstract class Configuration<T> where T : Configuration<T>
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => ConfigurationManager<T>.Instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watched">need to watch file changed or not</param>
        protected Configuration(bool watched)
        {
            this.IsWatched = watched;
        }

        public static T Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public bool IsWatched { get; private set; }

        public string ConfigurationFolder { get; set; }

        public abstract void Load();

        protected virtual U Deserialize<U>(string relatedCofigFile)
        {
            return XmlSerializerHelper.ToObj<U>(File.ReadAllText(Path.Combine(this.ConfigurationFolder, relatedCofigFile)));
        }
    }
}
