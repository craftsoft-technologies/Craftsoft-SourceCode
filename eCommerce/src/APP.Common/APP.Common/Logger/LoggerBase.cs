namespace APP.Common.Logger
{
    using System;
    using log4net;

    /// <summary>
    /// Provide Logging Functionality.
    /// </summary>
    public class LoggerBase
    {
        private ILog _logger;
        internal LoggerBase(ILog logger)
        {
            _logger = logger;
        }

        private LoggerBase() { }

        /// <summary>
        /// Log message as Debug level
        /// </summary>
        /// <param name="message">The message to log.</param>
        public virtual void Debug(string message)
        {
            _logger.Debug(message);
        }

        /// <summary>
        /// Log message as Debug level
        /// </summary>
        public virtual void DebugFormat(string format, params object[] args)
        {
            _logger.DebugFormat(format, args);
        }

        /// <summary>
        /// Log message as Debug level
        /// </summary>
        /// <param name="condition">if condition return true to cause a message to be written; otherwise, false. </param>
        /// <param name="message">The message to log.</param>
        public virtual void DebugIf(Func<bool> condition, string message)
        {
            if (condition != null)
            {
                DebugIf(condition(), message);
            }
        }

        /// <summary>
        /// Log message as Debug level
        /// </summary>
        /// <param name="condition">if condition is true to cause a message to be written; otherwise, false. </param>
        /// <param name="message">The message to log.</param>        
        public virtual void DebugIf(bool condition, string message)
        {
            if (condition)
            {
                _logger.Debug(message);
            }
        }

        /// <summary>
        /// Log message as Info level
        /// </summary>
        /// <param name="message">The message to log.</param>
        public virtual void Info(string message)
        {
            _logger.Info(message);
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            _logger.InfoFormat(format, args);
        }

        /// <summary>
        /// Log Exception as Info level
        /// </summary>
        /// <param name="exception">The Exception to log.</param>
        public virtual void InfoException(Exception exception)
        {
            _logger.Info(exception.Message, exception);

        }

        /// <summary>
        /// Log message as Warning level
        /// </summary>
        /// <param name="message">The message to log.</param>
        public virtual void Warn(string message)
        {
            _logger.Warn(message);
        }

        /// <summary>
        /// Log message as Error level
        /// </summary>
        /// <param name="message">The message to log.</param>
        public virtual void Error(string message)
        {
            _logger.Error(message);
        }

        public virtual void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }
        
        /// <summary>
        /// Log message as Exception(Fatal) level
        /// </summary>
        /// <param name="exception">The Exception to log.</param>
        public virtual void Exception(Exception exception)
        {
            _logger.Fatal(exception.Message, exception);

        }
    }
}
