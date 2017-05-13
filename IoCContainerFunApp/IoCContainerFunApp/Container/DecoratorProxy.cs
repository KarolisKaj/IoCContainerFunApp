using IoCContainerFunApp.Container.Attribute;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace IoCContainerFunApp.Container
{
    public class DecoratorProxy : RealProxy
    {
        private readonly object _instance;
        private Action<IMessage> _preAction;
        private Action<IMessage> _postAction;
        private DecoratorProxy(object instance, Action<IMessage> preAction = null, Action<IMessage> postAction = null)
            : base(instance.GetType())
        {
            _preAction = preAction;
            _postAction = postAction;
            _instance = instance;
        }

        public static object Create(object instance, Action<IMessage> preAction = null, Action<IMessage> postAction = null)
        {
            return new DecoratorProxy(instance, preAction, postAction).GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;
            _preAction?.Invoke(msg);
            var result = method.Invoke(_instance, methodCall.InArgs);
            _postAction?.Invoke(msg);
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
        }
    }
}
