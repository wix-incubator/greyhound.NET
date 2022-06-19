using System;
using greyhound.NET.SideCar;
using greyhound.NET.Domain;
using Microsoft.Extensions.Logging;
using greyhound.NET.SideCar.Producer;
using greyhound.NET.SideCar.Consumer;

namespace greyhound.NET.Client
{
    public sealed class GreyhoundSidecarClient : IDisposable
    {
        public static readonly string[] helpCommands = new string[] { "-h", "--help", "?" };
        public static readonly string help =
            "This is greyhound sidecar demo app\n" +
            "App usage: Command [<options>]\n" +
            "coomands:\n" +
            "create <topic-name> <topic partition>\n" +
            "produce <topic-name> <key> [<payload>]";

        public static readonly IReadOnlyDictionary<string, string> emptyHeaders = new Dictionary<string, string>();

        readonly GreyhoundProducer producer;
        readonly IConsumer consumer;
        readonly ILogger<GreyhoundSidecarClient> logger;


        public GreyhoundSidecarClient(string uri = "http://localhost:5287", string sidecarUri = "http://localhost:5288")
        {

            var loggerFactory = LoggerFactory.Create(builder =>
                builder
                .SetMinimumLevel(LogLevel.Information)
                .AddConsole()
                );

            logger = loggerFactory.CreateLogger<GreyhoundSidecarClient>();
            producer = new GreyhoundProducer(uri, loggerFactory);
            consumer = new GreyhoundConsumerBuilder(uri, sidecarUri, loggerFactory)
                .AddConsumer(new Consumer(Guid.NewGuid().ToString(), "my-group", "my-topic"), handler: ConsumerHandler)
                .Build();
        }

        private void ConsumerHandler(object? sender, Record record)
        {
            logger.LogInformation("client consumed topic `my-topic` with payload `{payload}`!!", record.Payload);
        }

        public void Run()
        {
            producer.CreateTopics(new CreateTopicsRequest(new Topic[]
            {
                new Topic("my-topic", 3)
            }));

            while (true)
            {
                var line = Console.ReadLine()?.Trim();
                if (line == ":q")
                    break;


                HandleCommand(line ?? "");


            }
        }

        private void HandleCommand(string args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var parts = args.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var command = parts.FirstOrDefault()?.ToLower();
            if (helpCommands.Contains(command))
                Console.WriteLine(help);
            else if (command == "create")
                HandleCreate(parts[1..]);
            else if (command == "produce")
                HandleProduce(parts[1..]);
            else
                Console.WriteLine("unrecognize command, type ? for help");




        }
        private void HandleProduce(string[] args)
        {
            var topic = args[0];
            var key = args[1];
            var payload = args.Length > 2 ? args[2] : null;
            producer.Produce(new ProduceRequest(topic, payload, key, emptyHeaders));
        }

        private void HandleCreate(string[] args)
        {
            var name = args[0];
            int? partitions = null;
            if (int.TryParse(args[1], out int partitionsParsed))
            {
                partitions = partitionsParsed;
            }
            producer.CreateTopics(new CreateTopicsRequest(new Topic[]
            {
                new Topic(name, partitions)
            }));

        }



        public void Dispose()
        {
            producer.Dispose();
            consumer.Dispose();
        }
    }
}

