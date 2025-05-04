using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTrigger.Services
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;
        private readonly string _connectionString;
        public QueueService(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task<string> RecieveMessageAsync()
        {
            var messageResponse = await _queueClient.ReceiveMessageAsync();
            if (messageResponse.Value != null)
            {
                string message = messageResponse.Value.Body.ToString();
                await _queueClient.DeleteMessageAsync(messageResponse.Value.MessageId, messageResponse.Value.PopReceipt);
                return message;
            }
            return "";
        }
      
        public async Task<List<string>> RecieveMessagesAsync()
        {
         
         
            var messagesResponse = await _queueClient.ReceiveMessagesAsync();

            var receivedMessages = new List<string>();

            foreach (var message in messagesResponse.Value)
            {
                string body = message.Body.ToString();
                receivedMessages.Add(body);

                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }

            return receivedMessages;
        }

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _queueClient.SendMessageAsync(message, TimeSpan.FromSeconds(15), TimeSpan.FromMinutes(10));
            }
        }
        public async Task SendMessageWithCountAsync(string message, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SendMessageAsync(message);
            }
        }
        public async Task<bool> Exists(string queueName)
        {
            var client = new QueueClient(_connectionString, queueName);
            return await client.ExistsAsync();
        }

    }
}
