using System;
using Com.Wixpress.Dst.Greyhound.Sidecar.Api;
using System.Threading.Tasks;
using Grpc.Core;

namespace greyhound.NET.SideCar
{
    public class GreyhoundConsumer : GreyhoundSidecarUser.GreyhoundSidecarUserBase, IConsumer
    {


        public GreyhoundConsumer()
        {

        }


        public override Task<HandleMessagesResponse> HandleMessages(HandleMessagesRequest request, ServerCallContext context)
        {
            return base.HandleMessages(request, context);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }


}

