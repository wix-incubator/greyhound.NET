using System.Linq;
using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.SideCar.Converters
{
    internal static class CreateTopicsRequestConvertes
    {
        private static Domain.Topic AsDomain(this Proto.TopicToCreate request) => new(name: request.Name, partitions: request.Partitions);

        public static Domain.CreateTopicsRequest AsDomain(this Proto.CreateTopicsRequest request) => new(topics: request.Topics.Select(topic => topic.AsDomain()));


        private static Proto.TopicToCreate AsProto(this Domain.Topic request) => new Proto.TopicToCreate { Name = request.Name, Partitions = request.Partitions };


        public static Proto.CreateTopicsRequest AsProto(this Domain.CreateTopicsRequest request) => new Proto.CreateTopicsRequest { Topics = { request.Topics.Select(topic => topic.AsProto()) } };


    }
}

