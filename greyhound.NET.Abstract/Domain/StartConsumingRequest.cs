using System.Collections.Generic;
using System.Linq;

namespace greyhound.NET.Domain
{
    public class StartConsumingRequest
    {
        public StartConsumingRequest(IEnumerable<Consumer> consumers)
        {
            Consumers = consumers?.ToList() ?? new List<Consumer>();
        }

        public IReadOnlyList<Consumer> Consumers { get; }
    }
}



