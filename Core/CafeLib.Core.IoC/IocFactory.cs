namespace CafeLib.Core.IoC
{
    public static class IocFactory
    {
        public static IServiceRegistry CreateRegistry()
        {
            return new ServiceRegistry();
        }
    }
}
