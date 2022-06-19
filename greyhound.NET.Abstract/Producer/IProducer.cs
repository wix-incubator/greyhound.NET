using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using greyhound.NET.Domain;

namespace greyhound.NET
{

    public interface IProducer: IDisposable
    {
        ProduceResponse Produce(ProduceRequest request);
        CreateTopicsResponse CreateTopics(CreateTopicsRequest request);
    }



}

