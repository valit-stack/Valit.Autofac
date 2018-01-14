using System.Reflection;
using Autofac;

namespace Valit.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterValit(this ContainerBuilder builder)
        {
            var assembly = Assembly.GetCallingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IValitator<>))
                .AsImplementedInterfaces()
                .SingleInstance();

            return builder;
        }
    }
}
