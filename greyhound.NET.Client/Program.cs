
using greyhound.NET.Client;
using greyhound.NET.Domain;
using greyhound.NET.SideCar;
using Microsoft.Extensions.Logging;

namespace greyhound.NET.Client
{
    public class Program
    {
        static ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static void Main(string[] args)
        {
            var sidecar = "http://localhost:5287";
            var local = "http://localhost:5287";
            var greyhound = new GreyhoundBuilder(sidecar)
                .WithLogger(loggerFactory)
                .WithProducer()
                .WithConsumer(local)
                .WithConsumeTopic("first-topic", "some-group", (s, r) => HandleTopic("first-topic", r))
                .WithConsumeTopic("second-topic", "some-group", (s, r) => HandleTopic("second-topic", r))
                .WithConsumeTopic("third-topic", "some-group", (s, r) => HandleTopic("third-topic", r))
                .Build();


            using var client = new GreyhoundSidecarClient();




            client.Run();

        }

        static void HandleTopic(string topic, Record r)
        {
            loggerFactory.CreateLogger<Greyhound>()
                .LogInformation("handling topic `{topic}` with key=`{key}, Payload=`{Payload}` ", topic, r.Key, r.Payload);
        }

    }

}




