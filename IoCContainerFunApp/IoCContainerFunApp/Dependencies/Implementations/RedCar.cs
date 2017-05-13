using IoCContainerFunApp.Dependencies.Interfaces;
using System;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class RedCar : ICar
    {
        public IService SomeService { get; set; }
        public RedCar(IService service)
        {
            SomeService = service;
        }
        public void Drive()
        {
            Console.WriteLine($"Started driving {this.ToString()}");
        }

        private IDriverProvider _driverProvider;
        public IDriverProvider SetDriver
        {
            get { return _driverProvider; }
            set
            {
                _driverProvider = value;
                _driver = (_driverProvider.Provide() as String);
            }
        }

        private string _driver;
        public string Driver => _driver;

        public override string ToString() => "Red Car";

    }
}
