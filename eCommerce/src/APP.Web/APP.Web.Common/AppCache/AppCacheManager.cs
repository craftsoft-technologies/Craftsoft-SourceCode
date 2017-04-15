using System;
using System.Collections.Generic;
using System.Threading;
using APP.Web.Common.Utilities;

namespace APP.Web.Common.AppCache
{
    public static class AppCacheManager
    {
        private static readonly List<Thread> Threads = new List<Thread>();
        private static readonly object SyncRoot = new object();

        public static void InitializeCache(Action loadAction, int loadIntervalSeconds, string name)
        {
            LogHelper.Server.InfoFormat(" AppCacheManager.InitializeCache: {0} {1}", name, loadIntervalSeconds);

            loadAction();
            //TODO
            var refreshThread = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    Thread.Sleep(loadIntervalSeconds * 1000);
                    LogHelper.Server.Info(" AppCacheManager.Load " + name);
                    loadAction();
                }
            }));
            refreshThread.Start();
            lock (SyncRoot)
            {
                Threads.Add(refreshThread);
            }
        }
    }
}
