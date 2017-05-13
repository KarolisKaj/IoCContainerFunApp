namespace IoCContainerFunApp.Container.Attribute
{
    using System;
    public class DemonDecoratorAttribute : Attribute
    {
        public DemonDecoratorAttribute(Type decoratorType, string preMethod = null, string postMethod = null)
        {
            DecoratorType = decoratorType;
            PreMethod = preMethod;
            PostMethod = postMethod;
        }
        public Type DecoratorType { get; set; }
        public string PreMethod { get; set; }
        public string PostMethod { get; set; }
    }
}
