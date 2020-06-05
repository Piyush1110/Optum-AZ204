using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusSubscriber
{
    class Program
    {
        private const string queueName = "items";
        private const string connectionString = "Endpoint=sb://sonusb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=PxQ/v2X00wGNeD6lb3IniMvYR6t+ZgnNusfjyf9rI0o=";
        private static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press ENTER to exit");
            await ReceiveMessagesAsync();

            Console.ReadLine();
        }

        static Task ReceiveMessagesAsync()
        {
            queueClient = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            MessageHandlerOptions options = new MessageHandlerOptions(ExceptionHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls=1,                
            };

            queueClient.RegisterMessageHandler(MessageHandlerAsync,options );
            return Task.CompletedTask;
        }

        private static async Task MessageHandlerAsync(Message message, CancellationToken token)
        {
            dynamic messageData = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Body));
            Console.WriteLine($"Message Id:{messageData.Id}, Text : {messageData.Text}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"Exception received:{args.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}
