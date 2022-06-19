using System;
using proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using greyhound.NET.Domain;
using System.Collections.Generic;
using Grpc.Core;
using System.Linq;

namespace greyhound.NET.SideCar
{
    public class GreyhoundConsumerBuilder : IConsumerBuilder
    {
        protected readonly GrpcChannel channel;
        protected readonly proto.GreyhoundSidecar.GreyhoundSidecarClient client;
        protected readonly Dictionary<string, ConsumerGroup> consumers = new();
        public GreyhoundConsumerBuilder(string sidecarUri, string localUri, ILoggerFactory logger) : this(new Uri(sidecarUri), new Uri(sidecarUri), logger)
        {

        }

        public GreyhoundConsumerBuilder(Uri sidecarUri, Uri localUri, ILoggerFactory logger)
        {

            var options = new GrpcChannelOptions
            {
                DisposeHttpClient = true,
                LoggerFactory = logger,
            };
            channel = GrpcChannel.ForAddress(sidecarUri, options);
            client = new proto.GreyhoundSidecar.GreyhoundSidecarClient(channel);
            LocalUri = localUri;
        }

        public Uri LocalUri { get; }

        public IConsumerBuilder AddConsumer(Consumer consumer, EventHandler<Record> handler)
        {
            if (consumer is null)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (!consumers.ContainsKey(consumer.Group))
            {
                consumers[consumer.Group] = new ConsumerGroup();
            }

            var consumerGroup = consumers[consumer.Group];

            if (!consumerGroup.Topics.ContainsKey(consumer.Topic))
            {
                consumerGroup.Topics[consumer.Topic] = new ConsumerTopic(handler);
            }
            else
            {
                throw new ArgumentException($"Consumer group {consumer.Group} already handle the topic ${consumer.Topic}");
            }

            return this;

        }

        public IConsumer Build()
        {
            var sidecar = new GreyhoundSidecarUserImpl(consumers);
            var server = new Server
            {
                Services = { Com.Wixpress.Dst.Greyhound.Sidecar.Api.GreyhoundSidecarUser.BindService(sidecar) },
                Ports = { new ServerPort(LocalUri.Host, LocalUri.Port, ServerCredentials.Insecure) }

            };
            server.Start();

            client.Register(new proto.RegisterRequest { Host = LocalUri.Host, Port = LocalUri.Port.ToString() });

            var protoConsumers = consumers
                .SelectMany(item => item.Value.Topics
                        .Select(g => g.Key)
                        .Select(topic =>
                        new proto.Consumer
                        {
                            Topic = topic,
                            Group = item.Key,
                            Id = Guid.NewGuid().ToString()
                        }));

            client.StartConsuming(new proto.StartConsumingRequest
            {
                Consumers = { protoConsumers }
            });

            throw new NotImplementedException();
        }
    }


}

