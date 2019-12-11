using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.Messaging
{
    public interface IMessageBroadcast
    {
        void Send(string topic, string message);
        void Send(string topic, object message);
        void Send(TopicEnum topic, string topicParameter, object message);
        void Send(TopicEnum topic, string topicParameter, string message);
        void Send(TopicEnum topic, string message, IMessageBroadcastParameter parameter);
        void Send(string topic, string message, IMessageBroadcastParameter parameter);

    }
}
