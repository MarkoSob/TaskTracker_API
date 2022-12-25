using Microsoft.Extensions.Logging;

namespace TaskTracker.Core.Extensions
{
    public static class ILoggerExtentions
    {
        public static void LogAndThrowException(this ILogger logger, Exception exception)
        {
            logger?.LogError(exception, exception.Message);
            throw exception;
        }

        public static void LogMessageAndThrowException(this ILogger logger, string message, Exception exception)
        {
            logger?.LogError(exception, message);
            throw exception;
        }
    }
}
