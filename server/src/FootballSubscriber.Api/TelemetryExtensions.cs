using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FootballSubscriber.Api;

public static class TelemetryExtensions
{
    public static void AddOpenTelemetryLogging(
        this ILoggingBuilder logging,
        IWebHostEnvironment environment,
        string endpoint
    )
    {
        logging.AddOpenTelemetry(options =>
        {
            options.ConfigureResource(r =>
            {
                r.AddService("football-subscriber-server");
            });

            if (environment.IsDevelopment())
            {
                options.AddConsoleExporter();
            }
            else
            {
                options.AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(endpoint);
                    o.Protocol = OtlpExportProtocol.Grpc;
                });
            }
        });
    }

    public static void AddOpenTelemetryTracing(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        string endpoint
    )
    {
        services.AddOpenTelemetryTracing(options =>
        {
            options.ConfigureResource(r =>
            {
                r.AddService("football-subscriber-server");
            });
            options.AddAspNetCoreInstrumentation();

            if (environment.IsDevelopment())
            {
                options.AddConsoleExporter();
            }
            else
            {
                options.AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(endpoint);
                    o.Protocol = OtlpExportProtocol.Grpc;
                });
            }
        });
    }
}
