using System;
using Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using System.Threading.Tasks;
using Grpc.Core;
using System.Collections.Generic;
using greyhound.NET.SideCar.Converters;

namespace greyhound.NET.SideCar.Consumer
{
    public class GreyhoundSidecarUserImpl : GreyhoundSidecarUser.GreyhoundSidecarUserBase
    {
        public class SidecarGroups
        {
            public SidecarGroups(IReadOnlyDictionary<string, ConsumerGroup> groups)
            {
                Groups = groups;
            }

            public IReadOnlyDictionary<string, ConsumerGroup> Groups { get; }
        }
        public GreyhoundSidecarUserImpl(SidecarGroups groups)
        {
            Groups = groups?.Groups ?? new Dictionary<string, ConsumerGroup>();
        }

        public IReadOnlyDictionary<string, ConsumerGroup> Groups { get; }

        public async override Task<HandleMessagesResponse> HandleMessages(HandleMessagesRequest request, ServerCallContext context)
        {
            var handler = GetHandler(request.Group, request.Topic);
            try
            {
                foreach (var record in request.Records)
                {
                    handler?.Invoke(this, record.AsDomain());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Status = new Status(StatusCode.PermissionDenied, "Failed to handle greyhound record", ex);
            }
            return new HandleMessagesResponse();

        }

        private EventHandler<Domain.Record> GetHandler(string group, string topic)
        {
            if(Groups.TryGetValue(group, out var consumerGroup))
            {
                if(consumerGroup.Topics.TryGetValue(topic, out var topicHandler))
                {
                    return topicHandler.Handler;
                }
            }
            return (s, e) => { return; };
        }

    }


}

