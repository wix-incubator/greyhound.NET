using System;
using proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using greyhound.NET.Domain;
using System.Collections.Generic;
using Grpc.Core;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace greyhound.NET.SideCar.Consumer
{
    public class GreyhoundConsumerBuilder : IConsumerBuilder
    {
        protected readonly Dictionary<string, ConsumerGroup> consumers = new();
        private readonly Uri sidecarUri;
        private readonly ILoggerFactory logger;

        public GreyhoundConsumerBuilder(string sidecarUri, string localUri, ILoggerFactory logger) : this(new Uri(sidecarUri), new Uri(localUri), logger)
        {

        }

        public GreyhoundConsumerBuilder(Uri sidecarUri, Uri localUri, ILoggerFactory logger)
        {


            this.sidecarUri = sidecarUri;
            LocalUri = localUri;
            this.logger = logger;
        }

        public Uri LocalUri { get; }

        public IConsumerBuilder AddConsumer(Domain.Consumer consumer, EventHandler<Record> handler)
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

        private Task CreateServer()
        {


            var builder = WebApplication.CreateBuilder();

            var ip = Dns.GetHostAddresses(LocalUri.Host).FirstOrDefault(IPAddress.Loopback);
            
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(ip, LocalUri.Port, o => o.Protocols = HttpProtocols.Http2);
            });

            builder.Services.AddGrpc();
            builder.Logging.AddConsole();
            builder.Services.AddSingleton(new GreyhoundSidecarUserImpl.SidecarGroups(consumers));
            var app = builder.Build();

            app.MapGrpcService<GreyhoundSidecarUserImpl>();

            return app.RunAsync();
        }

        public IConsumer Build()
        {
            var serverTask = CreateServer();

            var options = new GrpcChannelOptions
            {
                DisposeHttpClient = true,
                LoggerFactory = logger,
            };
           using var channel = GrpcChannel.ForAddress(sidecarUri, options);
           var client = new proto.GreyhoundSidecar.GreyhoundSidecarClient(channel);

            client.Register(new proto.RegisterRequest { Host = "http://" + LocalUri.Host, Port = LocalUri.Port.ToString() });

            var protoConsumers = consumers
                .SelectMany(item => item.Value.Topics
                        .Select(g => g.Key)
                        .Select(topic =>
                        new proto.Consumer
                        {
                            Topic = topic,
                            Group = item.Key,
                            Id = Guid.NewGuid().ToString()
                        })).ToList();
            var request = new proto.StartConsumingRequest
            {
                Consumers = { protoConsumers }
            };

            client.StartConsuming(request);

            return new GreyhoundConsumer(serverTask);
        }
    }


}

