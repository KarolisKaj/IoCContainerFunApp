using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace IoCContainerFunApp.Container
{
    public class LogProxy : RealProxy
    {
        private readonly object _instance;
        private Action<string> _preAction;
        private Action<string> _postAction;
        private LogProxy(object instance, Action<string> preAction = null, Action<string> postAction = null)
            : base(instance.GetType())
        {
            _preAction = preAction;
            _postAction = postAction;
            _instance = instance;
        }

        public static object Create(object instance, Action<string> preAction = null, Action<string> postAction = null)
        {
            return new LogProxy(instance, preAction, postAction).GetTransparentProxy();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;
            _preAction($"{method} called on type - {_instance.GetType()}");
            var result = method.Invoke(_instance, methodCall.InArgs);
            _postAction($"{method} called on type - {_instance.GetType()}");
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
        }
    }
}
