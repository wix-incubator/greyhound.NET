using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.SideCar.Converters
{
    internal static class CreateTopicsResponseConverters
    {
        public static Domain.CreateTopicsResponse AsDomin(this Proto.CreateTopicsResponse _) => new Domain.CreateTopicsResponse();


        public static Proto.CreateTopicsResponse AsProto(this Domain.CreateTopicsResponse _) => new Proto.CreateTopicsResponse();


    }
}

