using System.Threading.Tasks;
using greyhound.NET.Domain;

namespace greyhound.NET
{

    public interface IGreyhoundProducer
    {
        ProduceResponse Produce(ProduceRequest request);

        CreateTopicsResponse CreateTopics(CreateTopicsRequest request);
    }
}

