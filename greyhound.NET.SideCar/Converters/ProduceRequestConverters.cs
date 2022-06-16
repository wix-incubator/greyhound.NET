using System;
using System.Linq;
using Google.Protobuf.Collections;
using Domain = greyhound.NET.Domain;
using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.SideCar.Converters
{
    internal static class ProduceRequestConverters
    {
        public static Domain.ProduceRequest AsDomin(this Proto.ProduceRequest request)
        {
            return new Domain.ProduceRequest(
                topic: request.Topic,
                payload: request.Payload,
                key: request.Key,
                customHeaders: request.CustomHeaders.ToDictionary(item => item.Key, item => item.Value)
                );
        }

        public static Proto.ProduceRequest AsProto(this Domain.ProduceRequest request)
        {
            return new Proto.ProduceRequest
            {
                Topic = request.Topic ?? string.Empty,
                Payload = request.Payload,
                Key = request.Key,
                CustomHeaders = { request.CustomHeaders.ToDictionary(item => item.Key, item => item.Value) }
            };
        }

    }

}

