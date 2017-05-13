using IoCContainerFunApp.Dependencies.Interfaces;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class LoggerDecorator
    {
        private readonly ILogger _logger;
        public LoggerDecorator(ILogger logger)
        {
            _logger = logger;
        }
        public void PreMethod(string message)
        {
            _logger.Log(@"Decorator PreMethod called with - {message}");
        }
        public void PostMethod(string message)
        {
            _logger.Log(@"Decorator PostMethod called with - {message}");
        }
    }
}
