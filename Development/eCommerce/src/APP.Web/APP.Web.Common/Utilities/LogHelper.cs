using System;
using log4net;

namespace APP.Web.Common.Utilities
{
    //PerformanceLog
    //  Request, API

    //Performance-API
    //  Level:Info

    //Performance-HttpRequest
    //  Level:Info

    //User
    //  Level:Info 

    //Security
    //  Level:warning

    //Exception
    //  Level:Exception

    //Server
    //  Level:Info
    //  Logger:server

    public static class LogHelper
    {
        //OFF > FATAL > ERROR > WARN > INFO > DEBUG > ALL 
        public static ILog Performance { get; private set; }
        public static ILog Server { get; private set; }
        public static ILog User { get; private set; }
        public static ILog ExceptionLog { get; private set; }
        public static ILog Integration { get; private set; }

        static LogHelper()
        {
            //log4net.Config.XmlConfigurator.Configure();

            Server = LogManager.GetLogger("Server");
            Performance = LogManager.GetLogger("Performance");
            ExceptionLog = LogManager.GetLogger("Exception");
            User = LogManager.GetLogger("User");
            Integration = LogManager.GetLogger("Integration");
        }

        private static readonly TimeSpan fastBound = new TimeSpan(0, 0, 1);
        public static void RequestPerformance(TimeSpan timeCost, string logInfo)
        {
            if (timeCost < fastBound)
            {
                Performance.InfoFormat("{0} [{1}]", logInfo, timeCost);
            }
            else
            {
                Performance.WarnFormat("{0} [{1}]", logInfo, timeCost);
            }
        }

        public static void Exception(Exception exception)
        {
            ExceptionLog.Error(string.Empty, exception);
        }

        /// <summary>
        /// User this method excute method, will log method performance
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="fun"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static TResult Excute<TResult>(Func<TResult> fun, string log)
        {
            DateTime start = DateTime.Now;
            TResult result;
            string exceptionMessage = null;
            try
            {
                result = fun();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                throw;
            }
            finally
            {
                RequestPerformance(DateTime.Now - start, log + exceptionMessage);
            }
            return result;
        }

        /// <summary>
        /// User this method excute method, will log method performance
        /// </summary>
        /// <param name="function"></param>
        /// <param name="log"></param>
        public static void Excute(Action function, string log)
        {
            DateTime start = DateTime.Now;
            string exceptionMessage = null;
            try
            {
                function();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                throw;
            }
            finally
            {
                RequestPerformance(DateTime.Now - start, log + exceptionMessage);
            }
            return;
        }
    }
}
