using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreStart.Messaging
{
    public class MessageBroadcastEventGrid : IMessageBroadcast
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string url = "http://localhost:7071/runtime/webhooks/EventGrid?functionName=Function1";

        public MessageBroadcastEventGrid()
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("aeg-event-type", "Notification");

        }

        public async Task Send(string topic, string message)
        {
            var e = new BroadcastEvent(topic.ToString(), "", message, "V1");
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

        }

        public async Task Send(string topic, object message)
        {
            var e = new BroadcastEvent(topic.ToString(), "", message, "V1");
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
        }

        public async Task Send(TopicEnum topic, string topicParameter, object message)
        {
            var e = new BroadcastEvent(topic.ToString(), topicParameter, message, "V1");
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
        }

        public async Task Send(TopicEnum topic, string topicParameter, string message)
        {
            var e = new BroadcastEvent(topic.ToString(), topicParameter, message, "V1");
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
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

    internal class BroadcastEvent
    {
        public BroadcastEvent()
        {
        }

        public BroadcastEvent(string eventType, string subject, object data, string dataVersion)
        {
            this.eventType = eventType;
            this.subject = subject;
            this.data = data;
            this.dataVersion = dataVersion;
        }

        public string id { get; set; } = Guid.NewGuid().ToString();
        public string eventType { get; set; } = "";
        public string subject { get; set; } = "";
        public DateTime eventTime { get; set; } = DateTime.Now;
        public object data { get; set; } = "";
        public string dataVersion { get; set; } = "";
    }
}
