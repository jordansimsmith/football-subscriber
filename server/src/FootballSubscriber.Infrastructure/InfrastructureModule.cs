using Autofac;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Infrastructure.Data;
using FootballSubscriber.Infrastructure.Services;

namespace FootballSubscriber.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<FixtureApiService>()
                .As<IFixtureApiService>()
                .InstancePerLifetimeScope();
        }
    }
}