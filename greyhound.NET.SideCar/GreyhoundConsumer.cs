using System;
using Grpc.Core;

namespace greyhound.NET.SideCar
{
    public class GreyhoundConsumer : IConsumer
    {
        public GreyhoundConsumer(Server server)
        {
            Server = server;
        }

        public Server Server { get; }

        public void Dispose()
        {
            Server.ShutdownAsync().Wait();
        }
    }


}

