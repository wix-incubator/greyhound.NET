using Grpc.Core;
using Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.Server.Services;

public class GreyhoundSidecarImpl : GreyhoundSidecar.GreyhoundSidecarBase
{
    static readonly Dictionary<string, TopicToCreate> topics = new Dictionary<string, TopicToCreate>();
    private readonly ILogger logger;

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
        }
        else
        {
            logger.LogError("Topic `{topic}` never created in producer", request.Topic);
        }

        return new ProduceResponse();

    }
}

