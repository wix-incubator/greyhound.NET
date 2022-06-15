using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.Converters
{
    internal static class ProduceResponseConverters
    {
        public static Domain.ProduceResponse AsDomin(this Proto.ProduceResponse _) => new Domain.ProduceResponse();


        public static Proto.ProduceResponse AsProto(this Domain.ProduceResponse _) => new Proto.ProduceResponse();


    }
}

