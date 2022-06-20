using System.Collections.Generic;

namespace greyhound.NET.Domain
{
    public class Topic
    {
        public Topic(string name, int? partitions)
        {
            Name = name;
            Partitions = partitions;
        }

        public string Name { get; }
        public int? Partitions { get; }
    }

    

    public class Record
    {
        public Record(int partition, long offset, string payload, IReadOnlyDictionary<string, string> headers, string key)
        {
            Partition = partition;
            Offset = offset;
            Payload = payload;
            Headers = headers;
            Key = key;
        }

        public int Partition { get; }
        public long Offset { get; }
        public string Payload { get; }
        public IReadOnlyDictionary<string, string> Headers { get; }
        public string Key { get; }
    }
}

