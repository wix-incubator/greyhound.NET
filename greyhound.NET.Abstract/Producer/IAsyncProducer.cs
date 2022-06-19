using System.Threading.Tasks;
using greyhound.NET.Domain;

namespace greyhound.NET
{
    public interface IAsyncProducer
    {
        Task<CreateTopicsResponse> CreateTopicsAsync(CreateTopicsRequest request);
        Task<ProduceResponse> ProduceAsync(ProduceRequest request);
    }
}