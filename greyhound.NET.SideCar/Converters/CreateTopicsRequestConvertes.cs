using System.Linq;
using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.SideCar.Converters
{
    internal static class CreateTopicsRequestConvertes
    {
        private static Domain.TopicToCreate AsDomin(this Proto.TopicToCreate request) => new(name: request.Name, partitions: request.Partitions);

        public static Domain.CreateTopicsRequest AsDomin(this Proto.CreateTopicsRequest request) => new(topics: request.Topics.Select(topic => topic.AsDomin()));


        private static Proto.TopicToCreate AsProto(this Domain.TopicToCreate request) => new Proto.TopicToCreate { Name = request.Name, Partitions = request.Partitions };


        public static Proto.CreateTopicsRequest AsProto(this Domain.CreateTopicsRequest request) => new Proto.CreateTopicsRequest { Topics = { request.Topics.Select(topic => topic.AsProto()) } };


    }
}

