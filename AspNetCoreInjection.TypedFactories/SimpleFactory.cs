using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCoreInjection.TypedFactories
{
    /// <summary>
    /// Provides a way to defer or complete avoid the creation of instances.  Especially useful if the TService has been registered as a singleton
    /// since the TypedFactory does not support this scenario.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    internal class SimpleFactory<TService> : IFactory<TService> where TService : class
    {
        private readonly IServiceProvider serviceProvider;
        public SimpleFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TService Create() => serviceProvider.GetRequiredService<TService>();
    }
}
