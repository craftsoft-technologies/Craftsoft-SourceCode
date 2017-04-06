using System;
using System.Collections.Concurrent;
using System.Reflection;
using log4net;

namespace APP.Common.Config
{
    public class ConfigurationManager<T> where T : Configuration<T>
    {
        private static ILog log = LogManager.GetLogger("Server");
        private static readonly ConcurrentDictionary<string, T> cache = new ConcurrentDictionary<string, T>();

        public static T Instance
        {
            get
            {
                T instance;
                if (!cache.TryGetValue(typeof(T).FullName, out instance))
                {
                    string errorMessage = string.Format("No register of {0} in ConfigurationManager", typeof(T).ToString());
                    log.ErrorFormat(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                return (T)instance;
            }
        }

        public static void Register(string configFolder)
        {
            T instance = cache.GetOrAdd(
                typeof(T).FullName,
                (key) =>
                {
                    ConstructorInfo noParameterCtor = null;
                    ConstructorInfo[] constructorInfos = typeof(T).GetConstructors(
                        BindingFlags.Instance
                        | System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Public);
                    foreach (ConstructorInfo constructorInfo in constructorInfos)
                    {
                        if (constructorInfo.GetParameters().Length == 0)
                        {
                            noParameterCtor = constructorInfo;
                            break;
                        }
                    }
                    if (noParameterCtor == null)
                    {
                        throw new NotSupportedException("No constructor without parameter");
                    }

                    T t = (T)noParameterCtor.Invoke(null);
                    t.ConfigurationFolder = configFolder;
                    t.Load();

                    if (t.IsWatched)
                    {
                        ConfigFileWatcherHelper.AddToWatcher(
                            configFolder,
                            () =>
                            {
                                t.Load();
                            });
                    }

                    return t;
                });

            if (instance == null)
            {
                string errorMessage = string.Format("Failed to register {0} on ConfigurationManager", typeof(T).ToString());
                log.ErrorFormat(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
