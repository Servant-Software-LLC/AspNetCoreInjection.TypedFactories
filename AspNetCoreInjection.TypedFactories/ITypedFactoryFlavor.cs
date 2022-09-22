namespace AspNetCoreInjection.TypedFactories
{
    public interface ITypedFactoryFlavor
    {
        ITypedFactoryFlavor Flavor<TFrom, TTo>();

        ITypedFactoryFlavor Flavor<TConcrete>();

        void Register();
    }
}