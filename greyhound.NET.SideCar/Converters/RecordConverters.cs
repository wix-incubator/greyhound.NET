using System.Linq;
using Proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api;

namespace greyhound.NET.SideCar.Converters
{
    internal static class RecordConverters
    {
        public static Domain.Record AsDomain(this Proto.Record record) => new Domain.Record(
            record.Partition, record.Offset, record.Payload, record.Headers.ToDictionary(item => item.Key, item => item.Value), record.Key);


        public static Proto.Record AsProto(this Domain.Record record) => new Proto.Record
        {
            Partition = record.Partition,
            Offset = record.Offset,
            Payload = record.Payload,
            Headers = { record.Headers.ToDictionary(item => item.Key, item => item.Value) },
            Key = record.Key
        };


    }
}

