using System;
using System.IO;
using System.Linq;

namespace APP.Common.Logger
{
    public static class LoggerUtil
    {
        public static void InitializeLoggerConfiguration(string pathConfigFile)
        {
            log4net.GlobalContext.Properties["host"] = ((string[])AppDomain.CurrentDomain.FriendlyName.Split(':')).FirstOrDefault();
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(pathConfigFile, "Log4Net.config")));
        }

    }
}
