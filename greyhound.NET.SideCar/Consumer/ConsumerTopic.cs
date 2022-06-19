using System;
using Com.Wixpress.Dst.Greyhound.Sidecar.Api;

namespace greyhound.NET.SideCar.Consumer
{
    public sealed class ConsumerTopic
    {
        public EventHandler<Domain.Record> Handler { get; }

        public ConsumerTopic(EventHandler<Domain.Record> handler)
        {
            Handler = handler ?? ((s,e) => { });
        }
    }


}

