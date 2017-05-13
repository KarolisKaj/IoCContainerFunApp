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
        private object _delegateToInstance;
        private Type _interfaceToProxy;
        private DelegatorProxy(object instance, object delegateToObject, Type interfaceToProxy)
            : base(instance.GetType())
        {
            _delegateToInstance = delegateToObject;
            _interfaceToProxy = interfaceToProxy;
            _mainInstance = instance;
        }

        public static object Create(object proxyObject, object delegateToObject, Type interfaceToProxy)
        {
            return new DelegatorProxy(proxyObject, delegateToObject, interfaceToProxy).GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;
            object result;
            if (_interfaceToProxy.GetMethods().Any(x => x == method))
                result = method.Invoke(_delegateToInstance, methodCall.InArgs);
            else
                result = method.Invoke(_mainInstance, methodCall.InArgs);
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);

        }
    }
}
