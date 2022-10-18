using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading;

namespace MSAzureStorageQueueReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "ConnectString Or SAS Token";
            string queueName = "QueueName";

            QueueClient client = new QueueClient(connectionString, queueName);

            Console.WriteLine("Peeking Messages");

            while (true)
            {
                if (client.Exists())
                {
                    PeekedMessage[] messages = client.PeekMessages(32);

                    foreach (PeekedMessage message in messages)
                    {
                        Console.WriteLine($"{message.Body} | {message.MessageId} | {message.InsertedOn}");
                    }
                }

                if (client.Exists())
                {
                    QueueMessage[] messages = client.ReceiveMessages(32);

                    foreach (QueueMessage message in messages)
                    {
                        client.UpdateMessage(message.MessageId, message.PopReceipt, $"{message.Body} | {DateTime.Now}", TimeSpan.FromSeconds(5));
                    }
                }

                Console.WriteLine($"{DateTime.Now}");
                Thread.Sleep(7000);
                Console.WriteLine($"{DateTime.Now}");

                Console.WriteLine("Peeking Messages after Update");

                if (client.Exists())
                {
                    PeekedMessage[] messages = client.PeekMessages(32);

                    foreach (PeekedMessage message in messages)
                    {
                        Console.WriteLine($"{message.Body} | {message.MessageId} | {message.InsertedOn}");
                    }
                }

                if (client.Exists())
                {
                    QueueMessage[] messages = client.ReceiveMessages(32);

                    Console.WriteLine($"Deque Message: {messages[1].Body}");

                    foreach (var message in messages)
                    {

                        client.DeleteMessage(message.MessageId, message.PopReceipt);
                    }
                }

                Thread.Sleep(50000);
                Console.WriteLine("Peeking Messages after Delete");

                if (client.Exists())
                {
                    PeekedMessage[] messages = client.PeekMessages(32);

                    foreach (PeekedMessage message in messages)
                    {
                        Console.WriteLine($"{message.Body} | {message.MessageId} | {message.InsertedOn}");
                    }
                }
            }
        }
    }
}
