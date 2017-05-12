using System;
using System.Collections.Generic;

namespace IoCContainerFunApp.Container
{
    public class DemonContainer : IContainer
    {
        private Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();

        public void Compose()
        {
            throw new NotImplementedException();
        }

        public void Register<TImplementation, TAbstraction>(bool isLazy = false)
        {
            _registrations.Add(typeof(TImplementation), isLazy ? (Func<object>)(() => GetLazyInstance(typeof(TAbstraction))) : () => GetInstance(typeof(TAbstraction)));
        }

        private Lazy<object> GetLazyInstance(Type type)
        {

        }

        private object GetInstance(Type type)
        {

        }
    }
}
