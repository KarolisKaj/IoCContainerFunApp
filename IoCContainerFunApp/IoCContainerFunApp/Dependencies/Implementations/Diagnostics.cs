using IoCContainerFunApp.Container.Attribute;
using IoCContainerFunApp.Dependencies.Interfaces;
using System;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    [DemonDelegator(typeof(IPerformance), typeof(CpuDelegator))]
    public class Diagnostics : MarshalByRefObject, IDiagnosis
    {
        public Diagnostics()
        {

        }
        public double CurrentRamUsage => 80;
    }
}
