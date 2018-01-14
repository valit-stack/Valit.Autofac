using Autofac;
using Shouldly;
using Xunit;

namespace Valit.Autofac.Tests
{
    public class RegisterValitSingletonTests
    {
        [Fact]
        public void RegisterValitator_ResolveInstances_ReferencesShouldEqual()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterValit();

            var container = containerBuilder.Build();

            IValitator<Model> firstValitator;
            IValitator<Model> secondValitator;

            using(var scope = container.BeginLifetimeScope())
            {
                firstValitator = container.Resolve<IValitator<Model>>();
            }

            using(var scope = container.BeginLifetimeScope())
            {
                secondValitator = container.Resolve<IValitator<Model>>();
            }

            object.ReferenceEquals(firstValitator, secondValitator).ShouldBeTrue();
        }

        private class Model
        {
            public int Number { get; set; }
        }

        private class ModelValitator : IValitator<Model>
        {
            public IValitResult Validate(Model @object, IValitStrategy strategy = null)
                => ValitRules<Model>
                    .Create()
                    .Ensure(s => s.Number, s => s.IsGreaterThan(0))
                    .For(@object)
                    .Validate();
        }

    }
}
