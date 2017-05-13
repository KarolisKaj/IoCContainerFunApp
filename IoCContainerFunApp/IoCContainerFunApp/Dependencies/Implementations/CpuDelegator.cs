using IoCContainerFunApp.Dependencies.Interfaces;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class CpuDelegator : IPerformance
    {
        public double CurrentCPUUsage => 100;
    }
}
