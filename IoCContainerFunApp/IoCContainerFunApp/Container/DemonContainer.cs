using IoCContainerFunApp.Container.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace IoCContainerFunApp.Container
{
    public class DemonContainer : IContainer
    {
        private Dictionary<Type, object> _registrations = new Dictionary<Type, object>();

        public IEnumerable<Type> Parts => _registrations.Keys;

        public object this[Type type] { get => _registrations[type]; }

        public void Register<TAbstraction, TImplementation>(bool isLazy = false)
        {
            Register(typeof(TAbstraction), typeof(TImplementation), isLazy);
        }
        public void Register(Type typeAbstract, Type typeImplementation, bool isLazy = false)
        {
            _registrations.Add(typeAbstract, isLazy ? GetLazyInstance(typeImplementation) : GetInstance(typeImplementation));
        }
        public bool RegisterSafe(Type typeAbstract, Type typeConcrete, bool isLazy = false)
        {
            try
            {
                Register(typeAbstract, typeConcrete, isLazy);
                return true;
            }
            catch { return false; }
        }
        public bool RegisterSafe<TAbstraction, TImplementation>(bool isLazy)
        {
            try
            {
                Register<TAbstraction, TImplementation>(isLazy);
                return true;
            }
            catch { return false; }
        }

        // TODO: Perform lazy init
        // Lazy<object> was before. We need AoP approach
        private object GetLazyInstance(Type type) => GetInstance(type);

        private object GetInstance(Type type)
        {
            object instance = null;

            foreach (var ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                if (IsPossibleToCreate(ctor))
                {
                    instance = ctor.Invoke(MatchingDependenciesFor(ctor.GetParameters()).ToArray());
                    if (IsProxyRequired(instance))
                        instance = CreateAttributedProxy(instance);
                }
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

        private bool IsProxyRequired(object instance)
        {
            var attributes = GetKnownAttributesOfInstance(instance);
            return attributes.Count() > 0;
        }

        private object CreateAttributedProxy(object instance)
        {
            var attributes = GetKnownAttributesOfInstance(instance);
            if (attributes.OfType<DemonDecoratorAttribute>().Any())
            {
                var decoratorAttribute = attributes.OfType<DemonDecoratorAttribute>().First();
                RegisterSafe(decoratorAttribute.Type, decoratorAttribute.Type, true);
                var instanceTo = this[decoratorAttribute.Type];
                var preMethod = instanceTo.GetType().GetMethod(decoratorAttribute.PreMethod);
                var postMethod = instanceTo.GetType().GetMethod(decoratorAttribute.PostMethod);
                var actionType = typeof(Action<IMessage>);

                var preMethodDelegate = (Action<IMessage>)Delegate.CreateDelegate(actionType, instanceTo, preMethod);
                var postMethodDelegate = (Action<IMessage>)Delegate.CreateDelegate(actionType, instanceTo, postMethod);
                return DecoratorProxy.Create(instance, preMethodDelegate, postMethodDelegate);
            }
            if (attributes.OfType<DemonDelegatorAttribute>().Any())
            {
                var delegatorAttribute = attributes.OfType<DemonDelegatorAttribute>().First();

                return DelegatorProxy.Create(instance, GetInstance(delegatorAttribute.Implementation), delegatorAttribute.Abstraction);
            }
            throw new NotImplementedException("Missing imp");
        }

        private IEnumerable<System.Attribute> GetKnownAttributesOfInstance(object instance)
        {
            var allKnownAttributes = GetKnownAttributes();
            return instance.GetType().GetCustomAttributes().Where(x => allKnownAttributes.Any(y => y == x.GetType()));
        }

        private IEnumerable<Type> GetKnownAttributes()
        {
            return GetTypesInNamespace(this.GetType().Assembly, "IoCContainerFunApp.Container.Attribute").Where(x => x.BaseType == typeof(System.Attribute));
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
    }
}
