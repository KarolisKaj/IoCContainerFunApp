namespace IoCContainerFunApp.Container
{
    public interface IContainer
    {
        void Register<TImplementation, TAbstraction>(bool isLazy);
        void Compose();
    }
}
