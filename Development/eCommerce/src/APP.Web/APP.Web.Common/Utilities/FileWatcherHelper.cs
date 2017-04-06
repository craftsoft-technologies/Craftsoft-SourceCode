using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace APP.Web.Common.Utilities
{
    public class FileWatcherHelper
    {
        private static readonly List<FileWatcherHelper> WatcherHelpers = new List<FileWatcherHelper>();
        private readonly FileSystemWatcher watcher;
        private readonly Action callBackMethod;

        public FileWatcherHelper(string folder, Action callBackMethod)
        {
            Folder = folder;
            CallBackMethod = callBackMethod;
            watcher = new FileSystemWatcher(folder);
            WatcherHelpers.Add(this);
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            this.callBackMethod = callBackMethod;
            watcher.Changed += WatcherChanged;
        }

        public bool ScheduledLoading { get; set; }
        //for watching file update.
        public string Folder { get; private set; }
        public Action CallBackMethod { get; private set; }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        private void WatcherChanged(object sender, FileSystemEventArgs e)
        {
            (new Thread(() =>
            {
                if (!ScheduledLoading)
                {
                    LogHelper.Server.InfoFormat("{0} updated, will reload", Folder);
                    ScheduledLoading = true;
                    Thread.Sleep(5000);
                    //wait 5 seconds
                    ScheduledLoading = false;
                    LogHelper.Server.InfoFormat("start reload {0} ", Folder);
                    callBackMethod();
                }
                else
                {
                    LogHelper.Server.InfoFormat("{0} updated, do nothing", Folder);
                }
            })).Start();
        }
    }
}
