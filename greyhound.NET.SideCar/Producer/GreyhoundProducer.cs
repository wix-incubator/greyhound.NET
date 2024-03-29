﻿using System;
using proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using greyhound.NET.Domain;
using greyhound.NET.SideCar.Converters;
using System.Threading.Tasks;

namespace greyhound.NET.SideCar.Producer
{
    public class GreyhoundProducer : IProducer, IAsyncProducer
    {
        protected readonly GrpcChannel channel;
        protected readonly proto.GreyhoundSidecar.GreyhoundSidecarClient client;
        protected bool disposed = false;

        public GreyhoundProducer(string sidecarUri, ILoggerFactory logger) : this(new Uri(sidecarUri), logger)
        {

        }

        public GreyhoundProducer(Uri sidecarUri, ILoggerFactory logger)
        {
            var options = new GrpcChannelOptions
            {
                DisposeHttpClient = true,
                LoggerFactory = logger,
            };
            channel = GrpcChannel.ForAddress(sidecarUri, options);
            client = new proto.GreyhoundSidecar.GreyhoundSidecarClient(channel);

        }

        public ProduceResponse Produce(ProduceRequest request)
        {
            CheckDisposed();
            return client.Produce(request.AsProto()).AsDomain();
        }

        public CreateTopicsResponse CreateTopics(CreateTopicsRequest request)
        {
            CheckDisposed();
            return client.CreateTopics(request.AsProto()).AsDomain();
        }

        public async Task<CreateTopicsResponse> CreateTopicsAsync(CreateTopicsRequest request)
        {
            CheckDisposed();
            var response = await client.CreateTopicsAsync(request.AsProto());
            return response.AsDomain();
        }

        public async Task<ProduceResponse> ProduceAsync(ProduceRequest request)
        {
            CheckDisposed();
            var response = await client.ProduceAsync(request.AsProto());
            return response.AsDomain();
        }

        #region Dispose
        protected void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(this.GetType().FullName, "Unable to use object after dispose");
        }

        public void Dispose()
        {
            if (!disposed)
                Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                channel.Dispose();
            }
            disposed = true;
        }

        ~GreyhoundProducer()
        {
            Dispose(false);
        }
        #endregion


    }
}

