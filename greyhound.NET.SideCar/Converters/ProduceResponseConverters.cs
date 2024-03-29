﻿using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;

namespace greyhound.NET.SideCar.Converters
{
    internal static class ProduceResponseConverters
    {
        public static Domain.ProduceResponse AsDomain(this Proto.ProduceResponse _) => new Domain.ProduceResponse();


        public static Proto.ProduceResponse AsProto(this Domain.ProduceResponse _) => new Proto.ProduceResponse();


    }
}

