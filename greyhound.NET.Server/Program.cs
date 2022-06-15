using greyhound.NET.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace greyhound.NET.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            // Setup a HTTP/2 endpoint without TLS.
            options.ListenLocalhost(5287, o => o.Protocols =HttpProtocols.Http2);
        });
        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Logging.AddConsole();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreyhoundSidecarImpl>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
