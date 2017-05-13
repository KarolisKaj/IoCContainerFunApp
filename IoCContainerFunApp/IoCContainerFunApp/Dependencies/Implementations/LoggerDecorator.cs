using IoCContainerFunApp.Dependencies.Interfaces;
using System.Runtime.Remoting.Messaging;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class LoggerDecorator
    {
        private readonly ILogger _logger;
        public LoggerDecorator(ILogger logger)
        {
            _logger = logger;
        }
        public void PreMethod(IMessage msgInfo)
        {
            _logger.Log(@"Decorator PreMethod called with - {message}");
        }
        public void PostMethod(IMessage msgInfo)
        {
            _logger.Log(@"Decorator PostMethod called with - {message}");
        }
    }
}
