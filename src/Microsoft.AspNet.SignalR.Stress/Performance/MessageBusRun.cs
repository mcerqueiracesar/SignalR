﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Messaging;

namespace Microsoft.AspNet.SignalR.Stress
{
    public class MessageBusRun : RunBase
    {
        private const int MessageBufferSize = 10;
        private readonly MessageBus _bus;

        public MessageBusRun(int connections, int senders, string payload)
            : base(connections, senders, payload)
        {
            _bus = new MessageBus(Resolver);
        }

        protected override IDisposable CreateReceiver(int connectionIndex)
        {
            var subscriber = new Subscriber(connectionIndex.ToString(), new[] { "a", "b", "c" });
            return _bus.Subscribe(subscriber,
                                  cursor: null,
                                  callback: (result, state) => TaskAsyncHelper.True,
                                  maxMessages: MessageBufferSize,
                                  state: null);
        }

        protected override Task Send(int senderIndex)
        {
            return _bus.Publish(senderIndex.ToString(), "a", Payload);
        }

        public override void Dispose()
        {
            base.Dispose();

            _bus.Dispose();
        }
    }
}
