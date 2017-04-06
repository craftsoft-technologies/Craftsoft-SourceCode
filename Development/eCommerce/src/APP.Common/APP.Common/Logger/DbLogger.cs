using log4net;

namespace APP.Common.Logger
{
    public class DbLogger : LoggerBase
    {
        private DbLogger() : base(LogManager.GetLogger(typeof(DbLogger).Name)) { }

        private static DbLogger instance = new DbLogger();
        public static DbLogger Instance
        {
            get { return instance; }
        }

    }
}
