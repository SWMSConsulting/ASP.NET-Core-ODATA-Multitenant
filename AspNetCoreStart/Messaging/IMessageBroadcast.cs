using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.Messaging
{
    public interface IMessageBroadcast
    {
        Task Send(string topic, string message);
        Task Send(string topic, object message);
        Task Send(TopicEnum topic, string topicParameter, object message);
        Task Send(TopicEnum topic, string topicParameter, string message);
        Task Send(TopicEnum topic, string message, IMessageBroadcastParameter parameter);
        Task Send(string topic, string message, IMessageBroadcastParameter parameter);

    }
}
