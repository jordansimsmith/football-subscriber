using System;
using System.Security.Claims;
using FootballSubscriber.Api;
using FootballSubscriber.Api.Filters;
using FootballSubscriber.Core;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Infrastructure;
using FootballSubscriber.Infrastructure.Data;
using FootballSubscriber.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetryLogging(
    builder.Environment,
    builder.Configuration["OpenTelemetry:Endpoint"]
);

builder.Services.AddOpenTelemetryTracing(
    builder.Environment,
    builder.Configuration["OpenTelemetry:Endpoint"]
);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FootballSubscriber.Api", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please provide the bearer token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("location")
    );
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

builder.Services.AddHttpClient<IFixtureApiService, FixtureApiService>();

builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("FootballSubscriber"));
builder.Services.AddHangfireContext(builder.Configuration.GetConnectionString("Hangfire"));

builder.Services.AddHangfire(configuration =>
{
    configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Hangfire"));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var footballSubscriberContext = scope.ServiceProvider.GetService<FootballSubscriberContext>();
footballSubscriberContext!.Database.Migrate();

using var hangfireContext = scope.ServiceProvider.GetService<HangfireContext>();
hangfireContext!.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(
        c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FootballSubscriber.Api v1")
    );
}

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions
    {
        Authorization = new[]
        {
            new HangfireDashboardFilter(
                app.Configuration["Hangfire:Username"],
                app.Configuration["Hangfire:Password"]
            )
        }
    }
);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

GlobalJobFilters.Filters.Add(
    new AutomaticRetryAttribute { Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete }
);
RecurringJob.AddOrUpdate<IRefreshFixtureService>(x => x.RefreshFixturesAsync(), Cron.Hourly);

app.Run();
