using Grpc.Core;
using Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using Grpc.Net.Client;
using System.Text;

namespace greyhound.NET.Server.Services;

public class GreyhoundSidecarImpl : GreyhoundSidecar.GreyhoundSidecarBase
{
    static readonly Dictionary<string, TopicToCreate> topics = new Dictionary<string, TopicToCreate>();
    private readonly ILogger logger;


    private static Dictionary<string, string> consumingTopics = new();
    private static GreyhoundSidecarUser.GreyhoundSidecarUserClient? sideCarClient = null;


    public GreyhoundSidecarImpl(ILogger<GreyhoundSidecarImpl> logger)
    {
        this.logger = logger;
    }

    public override async Task<CreateTopicsResponse> CreateTopics(CreateTopicsRequest request, ServerCallContext context)
    {
        foreach (var topic in request.Topics)
        {
            if (topics.ContainsKey(topic.Name))
            {
                logger.LogWarning("topic {topicName} alredy exist in sidecar", topic.Name);
            }
            else
            {
                logger.LogInformation("adding topic {topicName} with {partions} partions", topic.Name, topic.Partitions);
                topics[topic.Name] = topic;
            }
        }


        return new CreateTopicsResponse();
    }



    public async override Task<ProduceResponse> Produce(ProduceRequest request, ServerCallContext context)
    {
        if (topics.ContainsKey(request.Topic))
        {
            logger.LogInformation("producing event on topic `{topic}` ", request.Topic);
            if (consumingTopics.ContainsKey(request.Topic))
            {
                logger?.LogInformation("send consumed message from sidecar to DOTNET!!!");
                var handleRequest = new HandleMessagesRequest
                {
                    Group = consumingTopics[request.Topic],
                    Topic = request?.Topic ?? "",
                    Records ={
                        new Record{
                        Offset = 1L,
                        Partition = 1,
                        Payload = "{}",}
                    }
                };
                sideCarClient?.HandleMessages(handleRequest);
            }
        }
        else
        {
            logger.LogError("Topic `{topic}` never created in producer", request.Topic);
            
        }

        return new ProduceResponse();

    }

    public async override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        var options = new GrpcChannelOptions
        {
            DisposeHttpClient = true,
        };

        logger?.LogInformation("processing request for register with host = `{host}` and port = `{port}`", request.Host, request.Port);

        var sidecarUri = new Uri($"http://{request.Host}:{request.Port}");
        var channel = GrpcChannel.ForAddress(sidecarUri, options);
        await channel.ConnectAsync();
        sideCarClient = new GreyhoundSidecarUser.GreyhoundSidecarUserClient(channel);
        return new RegisterResponse { RegistrationId = Guid.NewGuid().ToString() };
    }



    public async override Task<StartConsumingResponse> StartConsuming(StartConsumingRequest request, ServerCallContext context)
    {
        request.Consumers.ToList().ForEach(c => consumingTopics[c.Topic] = c.Group);
        var sb = new StringBuilder();
        sb.AppendLine($"consumingTopics [{request.Consumers.Count},{consumingTopics.Count}]= ");
        consumingTopics.ToList().ForEach(kv => sb.AppendLine($"{kv.Key} => {kv.Value}"));
        logger.LogInformation(sb.ToString());
        return new StartConsumingResponse();
    }


}

