using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSender
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://sonu-eventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MUn7Zo8jVbTj3+s9JFq1NsuDmlDgAfN0BgSxThhoGA8=";
        private const string eventHubName = "datahub";

        async static Task Main(string[] args)
        {
            Console.WriteLine("Press ENTER to start sending messages");
            Console.ReadLine();
            await SendMessagesAsync();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static async Task SendMessagesAsync()
        {
            await using (EventHubProducerClient producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {                
                for(var i=1;i <= 10; i++)
                {
                    using EventDataBatch dataBatch = await producerClient.CreateBatchAsync();
                    for (var j = 1; j <= 25; j++)
                    {
                        var dataBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
                        {
                            Id= $"{i}-{j}",
                            Message=$"Event message {j} in batch {i}"
                        }));
                        dataBatch.TryAdd(new EventData(dataBody));                        
                    }
                    await producerClient.SendAsync(dataBatch);
                    Console.WriteLine($"Batch {i} published");
                    await Task.Delay(250);
                }
                
            };

        }
    }
}
