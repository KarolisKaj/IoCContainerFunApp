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
            Container.Register<IDriverProvider, DriverProvider>(true);
            Container.Register<IService, ClockService>(true);
            Container.Register<ICar, RedCar>(false);
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