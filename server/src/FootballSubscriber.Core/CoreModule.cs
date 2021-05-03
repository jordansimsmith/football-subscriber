using Autofac;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Services;

namespace FootballSubscriber.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RefreshFixtureService>().As<IRefreshFixtureService>().InstancePerLifetimeScope();
        }
    }
}