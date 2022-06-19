using System.Collections.Generic;

namespace greyhound.NET.SideCar
{
    public sealed class ConsumerGroup
    {

        public Dictionary<string, ConsumerTopic> Topics { get; } = new Dictionary<string, ConsumerTopic>();

    }


}

