using Microsoft.Extensions.Logging;

namespace BookService.Application.Common.Diagnostics
{ 
    public static class AppLog
    {
        public static void Info(ILogger logger, string message, object? data = null)
        {
            logger.LogInformation("INFO: {Message} {@Data}", message, data);
        }

        public static void Warn(ILogger logger, string message, object? data = null)
        {
            logger.LogWarning("WARN: {Message} {@Data}", message, data);
        }

        public static void Error(ILogger logger, string message, object? data = null)
        {
            logger.LogError("ERROR: {Message} {@Data}", message, data);
        }
    }
}