using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.Messaging
{
    public class MessageBroadcastConsole : IMessageBroadcast
    {
        public async Task Send(string topic, string message)
        {
            Console.WriteLine($"TOPIC {topic}: {message}");
        }

        public async Task Send(string topic, object message)
        {
            Console.WriteLine($"TOPIC {topic}: {message}");
        }

        public async Task Send(TopicEnum topic, string topicParameter, object message)
        {
            Console.WriteLine($"TOPIC {topic}/{topicParameter}: {message}");
        }

        public async Task Send(TopicEnum topic, string topicParameter, string message)
        {
            Console.WriteLine($"TOPIC {topic}/{topicParameter}: {message}");
        }

        public async Task Send(TopicEnum topic, string message, IMessageBroadcastParameter parameter)
        {
            throw new NotImplementedException();
        }

        public async Task Send(string topic, string message, IMessageBroadcastParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}
