using System.Threading.Tasks;
using greyhound.NET.Domain;
using greyhound.NET.SideCar.Producer;

namespace greyhound.NET.SideCar
{
    public class Greyhound : IGreyhound, IAsyncGreyhound
    {
        private readonly GreyhoundProducer producer;
        private readonly IConsumer consumer;

        public Greyhound(GreyhoundProducer producer, IConsumer consumer)
        {
            this.producer = producer;
            this.consumer = consumer;
        }

        public CreateTopicsResponse CreateTopics(CreateTopicsRequest request) => producer?.CreateTopics(request);
        public async Task<CreateTopicsResponse> CreateTopicsAsync(CreateTopicsRequest request) => await producer?.CreateTopicsAsync(request);
        public ProduceResponse Produce(ProduceRequest request) => producer?.Produce(request);
        public async Task<ProduceResponse> ProduceAsync(ProduceRequest request) => await producer?.ProduceAsync(request);

        public void Dispose()
        {
            producer?.Dispose();
            consumer?.Dispose();
        }
    }
}

