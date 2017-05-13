using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IoCContainerFunApp.Container
{
    public class DemonContainer : IContainer
    {
        private Dictionary<Type, object> _registrations = new Dictionary<Type, object>();

        public IEnumerable<Type> Parts => _registrations.Keys;

        public void Compose()
        {
            throw new NotImplementedException();
        }

        public void Register<TImplementation, TAbstraction>(bool isLazy = false)
        {
            _registrations.Add(typeof(TImplementation), isLazy ? GetLazyInstance(typeof(TAbstraction)) : GetInstance(typeof(TAbstraction)));
        }

        private Lazy<object> GetLazyInstance(Type type) => new Lazy<object>(() => GetInstance(type), true);

        private object GetInstance(Type type)
        {
            object instance = null;
            foreach (var ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                if (IsPossibleToCreate(ctor))
                    return ctor.Invoke(MatchingDependenciesFor(ctor.GetParameters()).ToArray());
            }
            if (instance == null)
                throw new ArgumentException($"No suitable CTOR was found to create instance of {type}");
            InjectSetMethods(instance);
            return instance;
        }

        private IEnumerable<object> MatchingDependenciesFor(ParameterInfo[] arguments)
        {
            foreach (var argument in arguments)
            {
                yield return ResolveDependency(_registrations[argument.ParameterType]);
            }
        }

        private bool IsPossibleToCreate(ConstructorInfo ctorInfo)
        {
            return _registrations.Keys.Intersect(ctorInfo.GetParameters().Select(x => x.ParameterType)).Count() == ctorInfo.GetParameters().Count();
        }

        private void InjectSetMethods(object instance)
        {
            var propsToInject = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name.StartsWith("Set", StringComparison.InvariantCultureIgnoreCase));
            foreach (var propInfo in propsToInject)
            {
                propInfo.SetValue(instance, ResolveDependency(_registrations[propInfo.PropertyType]));
            }
        }

        private object ResolveDependency(object dependency)
        {
            if (dependency is Lazy<object>)
                return ((Lazy<object>)dependency).Value;
            return dependency;
        }
    }
}
