using System;
using greyhound.NET.Domain;

namespace greyhound.NET
{
    public interface IConsumerBuilder
    {
        public IConsumerBuilder AddConsumer(Consumer consumer, EventHandler<Record> handler);

        public IConsumer Build();
    }



}

