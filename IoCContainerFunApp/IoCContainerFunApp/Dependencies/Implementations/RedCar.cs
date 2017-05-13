using IoCContainerFunApp.Container.Attribute;
using IoCContainerFunApp.Dependencies.Interfaces;
using System;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    [DemonDecorator(typeof(LoggerDecorator), "PreMethod", "PostMethod")]
    public class RedCar : MarshalByRefObject, ICar
    {
        public IService SomeService { get; set; }
        public RedCar(IService service)
        {
            SomeService = service;
        }

        public void Drive()
        {
            Console.WriteLine($"Started driving {this.ToString()} {Environment.NewLine}With Driver {_driver}");
        }

        private IDriverProvider _driverProvider;
        public IDriverProvider SetDriver
        {
            get { return _driverProvider; }
            set
            {
                _driverProvider = value;
                _driver = (_driverProvider.Provide() as String);
                Drive();
            }
        }

        private string _driver;
        public string Driver => _driver;

        public override string ToString() => "Red Car";
    }
}
