﻿using System;
using System.Net;
using proto = Com.Wixpress.Dst.Greyhound.Sidecar.Api.V1;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using greyhound.NET.Domain;
using greyhound.NET.Converters;
using System.Threading.Tasks;

namespace greyhound.NET
{

    public class GreyhoundProducer : IGreyhoundProducer, IAsyncGreyhoundProducer, IDisposable
    {
        protected readonly GrpcChannel channel;
        protected readonly proto.GreyhoundSidecar.GreyhoundSidecarClient client;
        protected bool disposed = false;

        public GreyhoundProducer(string uri, ILoggerFactory logger): this(new Uri(uri), logger)
        {

        }

        public GreyhoundProducer(Uri uri, ILoggerFactory logger)
        {
            var options = new GrpcChannelOptions
            {
                DisposeHttpClient = true,
                LoggerFactory = logger,
            };
            channel = GrpcChannel.ForAddress(uri, options);
            client = new proto.GreyhoundSidecar.GreyhoundSidecarClient(channel);

        }

        public ProduceResponse Produce(ProduceRequest request)
        {
            CheckDisposed();
            return client.Produce(request.AsProto()).AsDomin();
        }

        public CreateTopicsResponse CreateTopics(CreateTopicsRequest request)
        {
            CheckDisposed();
            return client.CreateTopics(request.AsProto()).AsDomin();
        }

        public async Task<CreateTopicsResponse> CreateTopicsAsync(CreateTopicsRequest request)
        {
            CheckDisposed();
            var response = await client.CreateTopicsAsync(request.AsProto());
            return response.AsDomin();
        }

        public async Task<ProduceResponse> ProduceAsync(ProduceRequest request)
        {
            CheckDisposed();
            var response = await client.ProduceAsync(request.AsProto());
            return response.AsDomin();
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
