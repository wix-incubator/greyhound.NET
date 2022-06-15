using System.Threading.Tasks;
using greyhound.NET.Domain;

namespace greyhound.NET
{
    public interface IAsyncGreyhoundProducer
    {
        Task<CreateTopicsResponse> CreateTopicsAsync(CreateTopicsRequest request);
        Task<ProduceResponse> ProduceAsync(ProduceRequest request);
    }
}