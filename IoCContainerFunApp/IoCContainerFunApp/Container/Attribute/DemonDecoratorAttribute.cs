namespace IoCContainerFunApp.Container.Attribute
{
    using System;
    public class DemonDecoratorAttribute : Attribute
    {
        public DemonDecoratorAttribute(Type type, string preMethod = null, string postMethod = null)
        {
            Type = type;
            PreMethod = preMethod;
            PostMethod = postMethod;
        }
        public Type Type { get; set; }
        public string PreMethod { get; set; }
        public string PostMethod { get; set; }
    }
}
