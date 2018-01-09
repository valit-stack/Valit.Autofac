using System;
using Autofac;
using Xunit;
using Shouldly;

namespace Valit.Autofac.Tests
{
    public class RegisterAsSingletonTests
    {
        [Fact]
        public void Test1()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterValit();

            var container = containerBuilder.Build();

            var model = new Model{
                Number = 1
            };

            using(var scope = container.BeginLifetimeScope())
            {
                var reader = container.Resolve<IValitator<Model>>();

                var validateResult = reader.Validate(model);

                validateResult.Succeeded.ShouldBeTrue();
            }
        }
    }


    public class Model
    {
        public int Number { get; set; }
    }

    public class ModelValitator : IValitator<Model>
	{
		public IValitResult Validate(Model @object, IValitStrategy strategy = null)
			=> ValitRules<Model>
				.Create()
				.Ensure(s => s.Number, s => s.IsGreaterThan(0))
				.For(@object)
				.Validate();
	}
}
