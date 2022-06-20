
using greyhound.NET.Client;
using greyhound.NET.Domain;
using greyhound.NET.SideCar;
using Microsoft.Extensions.Logging;

namespace greyhound.NET.Client
{
    public class Program
    {
        static ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        static readonly string help =
            "This is greyhound sidecar demo app\n" +
            "App usage: Command [<options>]\n" +
            "coomands:\n" +
            "create <topic-name> <topic partition>\n" +
            "produce <topic-name> [<payload>] [<key>]";

        static readonly IReadOnlyDictionary<string, string> emptyHeaders = new Dictionary<string, string>();

        public static async Task Main(string[] _)
        {
            var sidecar = "http://localhost:9000";
            var local = "http://localhost:9001";


            var topics = new string[] { "topic" }.ToList();// "first-topic", "second-topic", "third-topic" }.ToList();
            var greyhoundBuilder = new GreyhoundBuilder(sidecar)
                .WithLogger(loggerFactory)
                .WithProducer()
                .WithConsumer(local);
            topics.ForEach(topic => greyhoundBuilder.WithConsumeTopic(topic, "some-group-" + Guid.NewGuid().ToString(), (s, r) => HandleTopic(topic, r)));;
            using var gh = greyhoundBuilder.Build();

            await gh.CreateTopicsAsync(new CreateTopicsRequest(topics.Select(t => new Topic(t, 5))));


            while (true)
            {
                var line = Console.ReadLine()?.Trim() ?? "";
                var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var command = parts.FirstOrDefault()?.ToLower();

                if (command == ":q")
                    break;

                var log = command switch
                {
                    "?" or "-h" or "--help" => help,
                    "produce" => HandleProduce(gh, parts[1..]),
                    _ => "unrecognize command, type ? for help"

                };

                Console.WriteLine(log);

            }
        }

        private static string HandleProduce(Greyhound gh, string[] args)
        {
            var topic = args[0];
            var payload = args.Length > 1 ? args[1] : null;
            var key = args.Length > 2 ? args[2]  : null;
            gh.Produce(new ProduceRequest(topic, payload, key, emptyHeaders));
            return $"done producing `{topic}`";
        }

        static void HandleTopic(string topic, Record r)
        {
            loggerFactory.CreateLogger<Greyhound>()
                .LogInformation("handling topic `{topic}` with key=`{key}` Payload=`{Payload}` ", topic, r.Key, r.Payload);
        }

    }

}




