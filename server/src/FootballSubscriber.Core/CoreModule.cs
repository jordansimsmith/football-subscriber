using System;
using Autofac;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Services;

namespace FootballSubscriber.Core;

public class CoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<RefreshFixtureService>().As<IRefreshFixtureService>().InstancePerLifetimeScope();

        builder.RegisterType<CompetitionService>().As<ICompetitionService>().InstancePerLifetimeScope();
        builder.RegisterType<FixtureService>().As<IFixtureService>().InstancePerLifetimeScope();
        builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
        builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().InstancePerLifetimeScope();

        builder.RegisterType<FixtureChangeNotificationService>()
            .As<IFixtureChangeNotificationService>()
            .InstancePerLifetimeScope();

        builder
            .RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
            .AsClosedTypesOf(typeof(IMerger<>))
            .AsImplementedInterfaces();
    }
}