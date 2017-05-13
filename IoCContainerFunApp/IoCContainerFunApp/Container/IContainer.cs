using System;
using System.Collections.Generic;

namespace IoCContainerFunApp.Container
{
    public interface IContainer
    {
        IEnumerable<Type> Parts { get; }
        void Register<TImplementation, TAbstraction>(bool isLazy);
        void Compose();

    }
}
