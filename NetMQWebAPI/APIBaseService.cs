namespace WebApplication1
{
    using Microsoft.Extensions.Logging;
    using NLog;
    public class APIBaseService
    {
        protected static Logger _logger = null;

        public APIBaseService()
        {
            var logger = LogManager.GetCurrentClassLogger();
            if (logger.IsEnabled(NLog.LogLevel.Debug))
            {
                _logger = logger;
            }
        }
    }
}