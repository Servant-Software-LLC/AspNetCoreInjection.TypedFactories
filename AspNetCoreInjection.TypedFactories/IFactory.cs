namespace AspNetCoreInjection.TypedFactories
{
    public interface IFactory<TService> where TService : class
    {
        TService Create();
    }
}

