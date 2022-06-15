namespace greyhound.NET.Domain
{
    public class TopicToCreate
    {
        public TopicToCreate(string name, int? partitions)
        {
            Name = name;
            Partitions = partitions;
        }

        public string Name { get; }
        public int? Partitions { get; }
    }
}

