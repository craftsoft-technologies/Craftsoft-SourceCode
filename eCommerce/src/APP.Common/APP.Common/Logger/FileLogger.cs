using System;
using log4net;
using System.Data;
using System.Data.SqlClient;

namespace APP.Common.Logger
{
    public class FileLogger : LoggerBase
    {
        private FileLogger() : base(LogManager.GetLogger(typeof(FileLogger).Name)) { }

        private static FileLogger instance = new FileLogger();
        public static FileLogger Instance
        {
            get { return instance; }
        }
    }
}
