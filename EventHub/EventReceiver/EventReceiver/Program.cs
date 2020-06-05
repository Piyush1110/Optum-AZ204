using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventReceiver
{
    class Program
    {

        private const string connectionString = "Endpoint=sb://sonu-eventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=laLOGU3eoDhzBRqGo3J3TLJhnfMaV0ZIULtAyLy+WoY=";
        private const string eventHubName = "myeventhub";
        private const string blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=sonustorageaccount;AccountKey=pYRwypXNTAm+YW6k1lr/56f5hI62I9/leoLKKAhXQzkhERakZF0OQIJQOK5coUiqg05v4TdhjJLXkZiWugLTmQ==;EndpointSuffix=core.windows.net";
        private const string blobContainerName = "files";

        async static Task Main(string[] args)
        {
            await ReceiveMessagesAsync();
            Console.WriteLine("Press Ctrl+C to exit");
            Console.ReadLine();
        }

        static async Task ReceiveMessagesAsync()
        {

            // Read from the default consumer group: $Default
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;            

            BlobContainerClient storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

            EventProcessorClient processor = new EventProcessorClient(storageClient, consumerGroup, connectionString, eventHubName);

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            await processor.StartProcessingAsync();

        }

        static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {            
            Console.WriteLine("\tRecevied event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

            // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {            
            Console.WriteLine($"\tPartition '{ eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
