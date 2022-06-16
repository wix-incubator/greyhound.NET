using System.Collections.Generic;
using System.Linq;

namespace greyhound.NET.Domain
{
    public class CreateTopicsRequest
    {
        public CreateTopicsRequest(IEnumerable<Topic> topics)
        {
            Topics = topics.ToList();
        }

        public IReadOnlyList<Topic> Topics { get; }
    }
}

