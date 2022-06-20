using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace greyhound.NET.SideCar.Consumer
{
    public class GreyhoundConsumer : IConsumer
    {
        public GreyhoundConsumer(Task server)
        {
            Server = server;
        }

        public Task Server { get; }

        public void Dispose()
        {
            Server.Wait();
        }
    }


}

