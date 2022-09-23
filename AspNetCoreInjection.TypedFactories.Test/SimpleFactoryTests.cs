using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreInjection.TypedFactories.Test
{
    public class SimpleFactoryTests
    {

        [Fact]
        public void RespectsServiceLifetime_Singleton()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddSingleton<SingletonTestService>();
            container.RegisterFactoryForRegisteredService<SingletonTestService>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                SingletonTestService firstInstance = svcProvider.GetRequiredService<IFactory<SingletonTestService>>().Create();
                SingletonTestService secondInstance = svcProvider.GetRequiredService<IFactory<SingletonTestService>>().Create();

                Assert.Same(firstInstance, secondInstance);
            }

        }

        [Fact]
        public void RespectsServiceLifetime_Transient()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddTransient<SingletonTestService>();
            container.RegisterFactoryForRegisteredService<SingletonTestService>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                SingletonTestService firstInstance = svcProvider.GetRequiredService<IFactory<SingletonTestService>>().Create();
                SingletonTestService secondInstance = svcProvider.GetRequiredService<IFactory<SingletonTestService>>().Create();

                Assert.NotSame(firstInstance, secondInstance);
            }

        }

    }
}
