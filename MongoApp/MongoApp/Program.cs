using MongoDB.Bson.IO;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MongoApp
{
    class Program
    {
        const string ConnectionString = "mongodb://sonu-mongo:41VYlZ4bgdwPrLkLxdpV5qPsU1MBdDuXrzTvSX4FzzLYoGUBdxev2iEhb9GRLa0j1noKTtFBMRVMQS2UsdIquw==@sonu-mongo.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@sonu-mongo@";

        static async Task Main(string[] args)
        {
            var order = new Order
            {
                CustomerName="Sonu",
                OrderDate=DateTime.Now,
                Amount = 1500,
                Email = "sonusathyadas@gmail.com",
                Approved=true
            };

            OrderService orderService = new OrderService(ConnectionString, "sampledb");
            var response=await orderService.InsertOrderAsync(order);
            Console.WriteLine($"Order inserted with Id {response.Id}");
            Console.ReadKey();
        }

        
    }
}
