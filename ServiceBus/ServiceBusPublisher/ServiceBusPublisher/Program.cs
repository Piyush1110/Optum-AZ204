using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusPublisher
{
    class Program
    {
        private const string queueName = "items";
        private const string connectionString = "Endpoint=sb://sonusb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Mlg+FsZrpSpLhuzQ8dcdhzkOJzIOFQCxyCbcF2LeFLY=";
        private static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            await SendMessagesAsync();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private async static Task SendMessagesAsync()
        {
            queueClient = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            for (var i = 1; i <= 10; i++)
            {
                var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
                {
                    Id = i,
                    Text = "Hello friends"
                }));
                await queueClient.SendAsync(new Message(messageBody));
                Console.WriteLine($"Message with id {i} sent");
                await Task.Delay(500);
            }
        }
    }
}
