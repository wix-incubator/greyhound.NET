namespace greyhound.NET.Domain
{
    public class Consumer
    {
        public Consumer(string id, string group, string topic)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Group = group ?? throw new System.ArgumentNullException(nameof(group));
            Topic = topic ?? throw new System.ArgumentNullException(nameof(topic));
        }

        public string Id { get; }
        public string Group { get; }
        public string Topic { get; }
    }
}



