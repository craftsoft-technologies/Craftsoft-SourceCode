using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using log4net;

namespace APP.Common.Config
{
    /// <summary>
    /// Just watch config file update
    /// </summary>
    internal static class ConfigFileWatcherHelper
    {
        private static ILog log = LogManager.GetLogger("Server");
        private static readonly object locker = new object();
        private static ConcurrentDictionary<string, ConfigurationFileWatcher> watcherCache = new ConcurrentDictionary<string, ConfigurationFileWatcher>();

        public static void AddToWatcher(string watchedFolder, Action changedCallback)
        {
            watcherCache.AddOrUpdate(
                watchedFolder,
                key =>
                {
                    ConfigurationFileWatcher watcher = new ConfigurationFileWatcher(watchedFolder);
                    watcher.ChangedCallback = new List<Action>() { changedCallback };

                    watcher.Changed += delegate (object sender, FileSystemEventArgs e)
                    {
                        if (watcher.Updating)
                        {
                            return;
                        }
                        lock (locker)
                        {
                            watcher.Updating = true;
                        }

                        new Thread(() =>
                        {
                            log.InfoFormat("FileWatcher - the file {0} [{1}]", e.Name, e.ChangeType.ToString());

                            foreach (var callback in watcher.ChangedCallback)
                            {
                                log.DebugFormat("FileWatcher - call {0}", callback.Target.ToString());
                                callback();
                            }

                            lock (locker)
                            {
                                watcher.Updating = false;
                            }
                        }).Start();
                    };
                    watcher.EnableRaisingEvents = true;

                    return watcher;
                },
                (key, value) =>
                {
                    while (value.Updating)
                    {
                        Thread.Sleep(5000);
                    }
                    value.ChangedCallback.Add(changedCallback);
                    return value;
                });
        }
    }
}
