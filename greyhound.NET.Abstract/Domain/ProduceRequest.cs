using System;
using System.Collections.Generic;

namespace greyhound.NET.Domain
{
    public class ProduceRequest
    {
        public ProduceRequest(string topic, string payload, string key, IReadOnlyDictionary<string, string> customHeaders)
        {
            Topic = topic ?? string.Empty;
            Payload = payload;
            Key = key ?? string.Empty;
            CustomHeaders = customHeaders;
        }

        public string Topic { get; }
        public string Payload { get; }
        public string Key { get; }
        public IReadOnlyDictionary<string, string> CustomHeaders { get; }

    }
}

