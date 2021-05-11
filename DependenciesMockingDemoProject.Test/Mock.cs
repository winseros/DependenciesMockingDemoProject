using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;
using Autofac.Extras.Moq;
using DependenciesMockingDemoProject.Web.DataLayer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Module = Autofac.Module;

namespace DependenciesMockingDemoProject.Test
{
    internal static class Mock
    {
        internal static AutoMock Auto()
        {
            return AutoMock.GetLoose(builder =>
            {
                builder.RegisterModule<DbContextModule>();
                builder.RegisterModule<LoggerModule>();
            });
        }

        private class DbContextModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);
                var factory =
                    new DbContextFactory(Options.Create(new DataLayerOptions
                        {ConnectionString = Db.AppConnectionString}), TestLogger.Factory);
                builder.RegisterInstance<IDbContextFactory>(factory);
            }
        }

        private class LoggerModule : Module
        {
            protected override void AttachToComponentRegistration(
                IComponentRegistryBuilder componentRegistry,
                IComponentRegistration registration)
            {
                base.AttachToComponentRegistration(componentRegistry, registration);
                registration.PipelineBuilding += RegistrationOnPipelineBuilding;
            }

            private static void RegistrationOnPipelineBuilding(object sender, IResolvePipelineBuilder builder)
            {
                static bool Match(ParameterInfo info, IComponentContext context)
                {
                    return typeof(ILogger).IsAssignableFrom(info.ParameterType);
                }

                static object Provide(ParameterInfo info, IComponentContext context)
                {
                    Type loggerType = typeof(Logger<>).MakeGenericType(info.Member.DeclaringType!);
                    ConstructorInfo constructor = loggerType.GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                        null, new[] {typeof(ILoggerFactory)}, null);
                    Debug.Assert(constructor != null);
                    object instance = constructor.Invoke(new object[] {TestLogger.Factory});
                    return instance;
                }

                builder.Use(PipelinePhase.RegistrationPipelineStart, (context, pipeline) =>
                {
                    context.ChangeParameters(context.Parameters.Union(new[] {new ResolvedParameter(Match, Provide)}));
                    pipeline(context);
                });
            }
        }
    }
}