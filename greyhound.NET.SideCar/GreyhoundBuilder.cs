using System;
using System.Collections.Generic;
using greyhound.NET.Domain;
using greyhound.NET.SideCar.Consumer;
using greyhound.NET.SideCar.Producer;
using Microsoft.Extensions.Logging;

namespace greyhound.NET.SideCar
{
    public class GreyhoundBuilder
    {
        GreyhoundProducer Producer;
        ILoggerFactory loggerFactory;
        GreyhoundConsumerBuilder builder;
        private readonly string sidecarUrl;
        readonly List<(Domain.Consumer consumer, EventHandler<Record> handler)> consumers = new();

        public GreyhoundBuilder(string sidecarUrl)
        {
            this.sidecarUrl = sidecarUrl;
        }

        public GreyhoundBuilder WithLogger(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            return this;
        }

        public GreyhoundBuilder WithProducer()
        {
            Producer = new GreyhoundProducer(sidecarUrl, loggerFactory);
            return this;
        }
        public GreyhoundBuilder WithConsumer(string localUrl)
        {
            builder = new GreyhoundConsumerBuilder(sidecarUrl, localUrl, loggerFactory);
            return this;
        }

        public GreyhoundBuilder WithConsumeTopic(string topic, string group, EventHandler<Record> handler)
        {
            consumers.Add(new(new Domain.Consumer(Guid.NewGuid().ToString(), group, topic), handler));
            return this;
        }

        public Greyhound Build()
        {
            if (builder is not null)
            {
                foreach (var (consumer, handler) in consumers)
                {
                    builder.AddConsumer(consumer, handler);
                }
            }

            return new Greyhound(Producer, builder?.Build());
        }

    }
}

