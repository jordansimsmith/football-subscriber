using System;
using System.Net.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FootballSubscriber.Core;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Infrastructure;
using FootballSubscriber.Infrastructure.Data;
using FootballSubscriber.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureModule>();
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
            
            // TODO: use migrations
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetService<FootballSubscriberContext>();
            context.Database.EnsureCreated();
        }
    }
}