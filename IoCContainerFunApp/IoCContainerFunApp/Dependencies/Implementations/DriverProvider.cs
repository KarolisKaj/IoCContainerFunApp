using IoCContainerFunApp.Dependencies.Interfaces;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class DriverProvider : IDriverProvider
    {
        public object Provide()
        {
            return "Generic Driver";
        }
    }
}
