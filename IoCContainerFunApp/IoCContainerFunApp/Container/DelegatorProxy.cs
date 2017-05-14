using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace IoCContainerFunApp.Container
{
    public class DelegatorProxy : RealProxy
    {
        private object _mainInstance;
        private Lazy<object> _delegateToInstance;
        private Type _interfaceToProxy;
        private DelegatorProxy(object instance, Lazy<object> delegateToInstance, Type interfaceToProxy)
            : base(instance.GetType())
        {
            _delegateToInstance = delegateToInstance;
            _interfaceToProxy = interfaceToProxy;
            _mainInstance = instance;
        }

        public static object Create(object proxyObject, Lazy<object> delegateToInstance, Type interfaceToProxy)
        {
            return new DelegatorProxy(proxyObject, delegateToInstance, interfaceToProxy).GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;
            object result;
            if (_interfaceToProxy.GetMethods().Any(x => x == method))
                result = method.Invoke(_delegateToInstance.Value, methodCall.InArgs);
            else
                result = method.Invoke(_mainInstance, methodCall.InArgs);
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);

        }
    }
}
