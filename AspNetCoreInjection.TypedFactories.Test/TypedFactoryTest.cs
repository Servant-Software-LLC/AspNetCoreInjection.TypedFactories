using Xunit;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreInjection.TypedFactories.Test
{
    public class TypedFactoryTest
    {
        [Fact]
        public void ResolveWithInjectedAndInvokerParameters()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddTransient<ITestDependency, TestDependency>();
            container.RegisterTypedFactory<ITestServiceFactory>().ForConcreteType<TestService>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                ITestService testSvc = svcProvider.GetRequiredService<ITestServiceFactory>().Create("ParamValue");

                Assert.Equal("ParamValue", testSvc.FactoryParam);
                Assert.NotNull(testSvc.InjectedDepedency);
            }
        }

        [Fact]
        public void ResolveFlavor()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddTransient<ITestDependency, TestDependency>();
            container.RegisterTypedFactory<ITestServiceFactory>()
                .Flavor<ITestService, TestService>()
                .Flavor<ITestServiceFlavor1, TestServiceFlavor1>()
                .Flavor<ITestServiceFlavor2, TestServiceFlavor2>()
                .Flavor<TestServiceFlavor3WithoutCustomInterface>()
                .Register();

            using (var svcProvider = container.BuildServiceProvider())
            {
                ITestService f1 = svcProvider.GetRequiredService<ITestServiceFactory>().CreateFlavor1("Flavor1");
                ITestService f2 = svcProvider.GetRequiredService<ITestServiceFactory>().CreateFlavor2("Flavor2");
                ITestService f3 = svcProvider.GetRequiredService<ITestServiceFactory>().CreateFlavor3("Flavor3");

                Assert.IsAssignableFrom<ITestServiceFlavor1>(f1);
                Assert.Equal("Flavor1", f1.FactoryParam);

                Assert.IsAssignableFrom<ITestServiceFlavor2>(f2);
                Assert.Equal("Flavor2", f2.FactoryParam);

                Assert.IsAssignableFrom<TestServiceFlavor3WithoutCustomInterface>(f3);
                Assert.Equal("Flavor3", f3.FactoryParam);
            }
        }

        [Fact]
        public void ResolveWithInjectedParametersOnly()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddTransient<ITestDependency, TestDependency>();
            container.RegisterTypedFactory<ITestServiceInjectedOnlyFactory>().ForConcreteType<TestServiceInjectedOnly>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                ITestServiceInjectedOnly testSvc = svcProvider.GetRequiredService<ITestServiceInjectedOnlyFactory>().Create();

                Assert.NotNull(testSvc.InjectedDepedency);
            }
        }

        [Fact]
        public void ResolveWithMissingDependecy()
        {
            IServiceCollection container = new ServiceCollection();
            container.RegisterTypedFactory<ITestServiceFactory>().ForConcreteType<TestService>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                Assert.Throws<InvalidOperationException>(() =>
                    svcProvider.GetRequiredService<ITestServiceFactory>().Create("ParamValue")
                );
            }
        }

        [Fact]
        public void ResolveBadParameterName()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddTransient<ITestDependency, TestDependency>();
            container.RegisterTypedFactory<ITestServiceFactoryBadParamName>().ForConcreteType<TestService>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                Assert.Throws<Exception>(() =>
                    svcProvider.GetRequiredService<ITestServiceFactoryBadParamName>().CreateBadParamName("BadParamValue")
                );
            }
        }

        [Fact]
        public void ResolveBadParameterType()
        {
            IServiceCollection container = new ServiceCollection();
            container.AddTransient<ITestDependency, TestDependency>();
            container.RegisterTypedFactory<ITestServiceFactoryBadParamType>().ForConcreteType<TestService>();

            using (var svcProvider = container.BuildServiceProvider())
            {
                Assert.Throws<Exception>(() =>
                    svcProvider.GetRequiredService<ITestServiceFactoryBadParamType>().CreateBadParamType(1)
                );
            }
        }


        //NOTE:  This scenario fails and hence the need for the SimpleFactory class.
        //
        //[Fact]
        //public void RespectsServiceLifetime_Singleton()
        //{
        //    IServiceCollection container = new ServiceCollection();
        //    container.AddSingleton<SingletonTestService>();
        //    container.RegisterTypedFactory<ISingletonTestServiceFactory>().ForConcreteType<SingletonTestService>();

        //    using (var svcProvider = container.BuildServiceProvider())
        //    {
        //        SingletonTestService firstInstance = svcProvider.GetRequiredService<ISingletonTestServiceFactory>().Create();
        //        SingletonTestService secondInstance = svcProvider.GetRequiredService<ISingletonTestServiceFactory>().Create();

        //        Assert.Same(firstInstance, secondInstance);
        //    }

        //}
    }
}
