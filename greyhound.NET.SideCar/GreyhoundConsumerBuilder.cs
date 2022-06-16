using System;
using proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using greyhound.NET.Domain;

namespace greyhound.NET.SideCar
{
    public class GreyhoundConsumerBuilder : IConsumerBuilder
    {
        protected readonly GrpcChannel channel;
        protected readonly proto.GreyhoundSidecar.GreyhoundSidecarClient client;

        public GreyhoundConsumerBuilder(string sidecarUri, ILoggerFactory logger) : this(new Uri(sidecarUri), logger)
        {

        }

        public GreyhoundConsumerBuilder(Uri sidecarUri, ILoggerFactory logger)
        {
            var options = new GrpcChannelOptions
            {
                DisposeHttpClient = true,
                LoggerFactory = logger,
            };
            channel = GrpcChannel.ForAddress(sidecarUri, options);
            client = new proto.GreyhoundSidecar.GreyhoundSidecarClient(channel);
        }


        public IConsumerBuilder AddConsumer(Consumer consumer, EventHandler<Domain.Record> handler)
        {
            throw new NotImplementedException();
        }

        public IConsumer Build()
        {
            throw new NotImplementedException();
        }
    }


}

