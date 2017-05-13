using System;
using System.Collections.Generic;

namespace IoCContainerFunApp.Container
{
    public interface IContainer
    {
        IEnumerable<Type> Parts { get; }
        void Register(Type typeAbstract, Type typeConcrete, bool isLazy);
        void Register<TImplementation, TAbstraction>(bool isLazy);
        bool RegisterSafe(Type typeAbstract, Type typeConcrete, bool isLazy);
        bool RegisterSafe<TImplementation, TAbstraction>(bool isLazy);
        object this[Type type] { get; }
    }
}
