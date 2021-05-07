using System;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using FootballSubscriber.Core;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Mappers;
using FootballSubscriber.Infrastructure;
using FootballSubscriber.Infrastructure.Data;
using FootballSubscriber.Infrastructure.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FootballSubscriber.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "FootballSubscriber.Api", Version = "v1"});
            });

            services.AddDbContext(Configuration.GetConnectionString("FootballSubscriber"));
            services.AddHttpClient<IFixtureApiService, FixtureApiService>();

            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    });
            });
            services.AddHangfireServer();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterAutoMapper(typeof(FixtureProfile).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FootballSubscriber.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                IsReadOnlyFunc = _ => true
            });

            // TODO: use migrations
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetService<FootballSubscriberContext>();
            context.Database.Migrate();

            ConfigureRecurringJobs();
        }

        private void ConfigureRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IRefreshFixtureService>(x => x.RefreshFixturesAsync(), "*/15 * * * *");
        }
    }
}