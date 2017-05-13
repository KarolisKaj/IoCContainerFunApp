namespace IoCContainerFunApp.Container.Attribute
{
    using System;
    public class DemonDelegatorAttribute : Attribute
    {
        public DemonDelegatorAttribute(Type abstraction, Type implementation)
        {
            Abstraction = abstraction;
            Implementation = implementation;
        }
        public Type Abstraction { get; set; }
        public Type Implementation { get; set; }
    }
}
