using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.Messaging
{
    public class MessageBroadcastConsole : IMessageBroadcast
    {
        public void Send(string topic, string message)
        {
            Console.WriteLine($"TOPIC {topic}: {message}");
        }

        public void Send(string topic, object message)
        {
            Console.WriteLine($"TOPIC {topic}: {message}");
        }

        public void Send(TopicEnum topic, string topicParameter, object message)
        {
            Console.WriteLine($"TOPIC {topic}/{topicParameter}: {message}");
        }

        public void Send(TopicEnum topic, string topicParameter, string message)
        {
            Console.WriteLine($"TOPIC {topic}/{topicParameter}: {message}");
        }

        public void Send(TopicEnum topic, string message, IMessageBroadcastParameter parameter)
        {
            throw new NotImplementedException();
        }

        public void Send(string topic, string message, IMessageBroadcastParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}
