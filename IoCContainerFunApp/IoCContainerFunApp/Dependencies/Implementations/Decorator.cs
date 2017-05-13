using IoCContainerFunApp.Dependencies.Interfaces;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class Decorator
    {
        private readonly ILogger _logger;
        public Decorator(ILogger logger)
        {
            _logger = logger;
        }
        public void PreCall()
        {

        }
    }
}
