using IoCContainerFunApp.Container;
using IoCContainerFunApp.Dependencies.Implementations;
using IoCContainerFunApp.Dependencies.Interfaces;

namespace IoCContainerFunApp
{
    internal class Bootstrapper
    {
        public IContainer Container { get; set; }
        public Bootstrapper()
        {
            Container = new DemonContainer();
            Container.Register<ILogger, Logger>(true);
            Container.Register<IDriverProvider, DriverProvider>(true);
            Container.Register<IService, ClockService>(true);
            // Not lazy
            Container.Register<ICar, RedCar>(false);
            Container.Register<IPerformance, Diagnostics>(true);
            Container.Register<IDiagnosis, Diagnostics>(true);
            System.Console.WriteLine((Container[typeof(IPerformance)] as IPerformance).CurrentCPUUsage);

        }

        internal void Compose()
        {
        }

        internal void Build()
        {
        }

        internal void Start()
        {
            var window = new MainWindow();
            window.Show();
            window.InjectParts(Container.Parts);
        }
    }
}