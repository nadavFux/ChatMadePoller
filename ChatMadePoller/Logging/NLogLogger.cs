using Microsoft.Extensions.Logging;
using NLog;

namespace Logging
{
    public class NLogLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return _logger.IsTraceEnabled;
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    return _logger.IsFatalEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (exception != null)
            {
                switch (logLevel)
                {
                    case Microsoft.Extensions.Logging.LogLevel.Trace:
                        _logger.Trace(exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Debug:
                        _logger.Debug(exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Information:
                        _logger.Info(exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Warning:
                        _logger.Warn(exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Error:
                        _logger.Error(exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Critical:
                        _logger.Fatal(exception, message);
                        break;
                    default:
                        _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _logger.Info(exception, message);
                        break;
                }
            }
            else
            {
                switch (logLevel)
                {
                    case Microsoft.Extensions.Logging.LogLevel.Trace:
                        _logger.Trace(message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Debug:
                        _logger.Debug(message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Information:
                        _logger.Info(message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Warning:
                        _logger.Warn(message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Error:
                        _logger.Error(message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Critical:
                        _logger.Fatal(message);
                        break;
                    default:
                        _logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        _logger.Info(message);
                        break;
                }
            }
        }
    }
}


