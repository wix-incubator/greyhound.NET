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
            options.ListenLocalhost(5287, o => o.Protocols =HttpProtocols.Http2);
        });

        builder.Services.AddGrpc();
        builder.Logging.AddConsole();
        var app = builder.Build();

        app.MapGrpcService<GreyhoundSidecarImpl>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
