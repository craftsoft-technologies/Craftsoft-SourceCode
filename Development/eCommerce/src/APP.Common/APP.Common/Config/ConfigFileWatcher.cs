using System;
using System.Collections.Generic;
using System.IO;

namespace APP.Common.Config
{
    public partial class ConfigurationFileWatcher : FileSystemWatcher
    {
        public ConfigurationFileWatcher(string configFolder)
            : base(configFolder, "*.config")
        {
            this.NotifyFilter = NotifyFilters.LastWrite;
        }

        public bool Updating { get; set; }

        public List<Action> ChangedCallback { get; set; }
    }
}
