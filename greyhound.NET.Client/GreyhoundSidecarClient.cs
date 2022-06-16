using System;
using greyhound.NET.SideCar;
using greyhound.NET.Domain;
using Microsoft.Extensions.Logging;

namespace greyhound.NET.Client
{
	public sealed class GreyhoundSidecarClient: IDisposable
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

		public GreyhoundSidecarClient(string uri = "http://localhost:5287")
		{

			var logger = LoggerFactory.Create(builder =>
				builder
				.SetMinimumLevel(LogLevel.Trace)
				.AddConsole()
				);


            producer = new GreyhoundProducer(uri, logger);
		}

		public void Run()
        {
			while(true)
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
			var payload = args.Length > 2 ?  args[2] : null;
			producer.Produce(new ProduceRequest(topic, payload, key, emptyHeaders));
		}

		private void HandleCreate(string[] args)
        {
			var name = args[0];
			int? partitions = null;
			if (int.TryParse( args[1],out int partitionsParsed ))
            {
				partitions = partitionsParsed;
            }
			producer.CreateTopics(new CreateTopicsRequest(new TopicToCreate[]
			{
				new TopicToCreate(name, partitions)
			})) ;

		}



		public void Dispose()
        {
			producer.Dispose();
        }
    }
}

