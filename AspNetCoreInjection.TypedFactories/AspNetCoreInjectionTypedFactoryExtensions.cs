namespace AspNetCoreInjection.TypedFactories
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// Defines extension methods for providing custom typed factories based on a factory interface.
    /// </summary>
    public static class AspNetCoreInjectionTypedFactoryExtensions
    {

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The holder object which facilitates the fluent interface.
        /// </returns>
        public static ITypedFactoryRegistration RegisterTypedFactory<TFactory>(this IServiceCollection container)
            where TFactory : class
        {
            if (!typeof(TFactory).IsInterface)
            {
                throw new ArgumentException("The factory contract does not represent an interface!");
            }

            return new TypedFactoryRegistration<TFactory>(container);
        }

        /// <summary>
        /// Registers a typed factory.
        /// </summary>
        /// <typeparam name="TFactory">
        /// The factory interface.
        /// </typeparam>
        /// <typeparam name="TConcreteType">
        /// The concrete type that the factory will instantiate.
        /// </typeparam>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The container to continue its fluent interface.
        /// </returns>
        public static IServiceCollection RegisterTypedFactory<TFactory, TConcreteType>(this IServiceCollection container)
            where TFactory : class
        {
            container.RegisterTypedFactory<TFactory>().ForConcreteType<TConcreteType>();

            return container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service that is elsewhere registered to register a IFactory<TService> of.  Especially useful if the TService has been registered as a singleton
        /// since the TypedFactory does not support this scenario.
        /// </typeparam>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The container to continue its fluent interface.
        /// </returns>
        public static IServiceCollection RegisterFactoryForRegisteredService<TService>(this IServiceCollection container) where TService : class
        {
            container.AddSingleton<IFactory<TService>>(serviceProvider => new SimpleFactory<TService>(serviceProvider));
            return container;
        }
    }
}