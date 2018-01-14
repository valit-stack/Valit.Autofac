using System;
using Autofac;
using Xunit;
using Shouldly;

namespace Valit.Autofac.Tests
{
    public class RegisterValitatorsTests
    {
        [Fact]
        public void RegisterValit_IValitors_Should_Be_Registered()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterValit();

            var container = containerBuilder.Build();

            using(var scope = container.BeginLifetimeScope())
            {
                var isRegistered = scope.IsRegistered(typeof(IValitator<Model>));
                isRegistered.ShouldBeTrue();
            }
        }

        [Fact]
        public void RegisterValit_ResolveValit_Should_Not_Be_Null()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterValit();

            var container = containerBuilder.Build();

            using(var scope = container.BeginLifetimeScope())
            {
                var validator = container.Resolve<IValitator<Model>>();
                validator.ShouldNotBeNull();
            }

            container.Dispose();
        }

        [Fact]
        public void RegisterValit_ResolveValitator_Validation_Should_Succeed()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterValit();

            var container = containerBuilder.Build();

            using(var scope = container.BeginLifetimeScope())
            {
                var valitator = container.Resolve<IValitator<Model>>();
                var validateResult = valitator.Validate(_model);

                validateResult.Succeeded.ShouldBeTrue();
            }

            container.Dispose();
        }

        public RegisterValitatorsTests()
        {
            _model = new Model
            {
                Number = 1
            };
        }

        private Model _model;

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
