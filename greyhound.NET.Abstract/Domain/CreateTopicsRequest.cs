using System.Collections.Generic;
using System.Linq;

namespace greyhound.NET.Domain
{
    public class CreateTopicsRequest
    {
        public CreateTopicsRequest(IEnumerable<TopicToCreate> topics)
        {
            Topics = topics.ToList();
        }

        public IReadOnlyList<TopicToCreate> Topics { get; }
    }
}

