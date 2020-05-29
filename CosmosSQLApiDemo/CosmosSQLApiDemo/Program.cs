using Microsoft.Azure.Cosmos;
using System;

namespace CosmosSQLApiDemo
{
    class Program
    {
        const string accountUri= "https://sonu-coresql.documents.azure.com:443/";
        const string accountKey = "TKwbB7d5C8YG211uYPySV7PDEJLSv7j9Zi1BVNQl7zKL2jJMMVarQ78N87bQuz8dnvmyOnj3B04LZmw4Xyq4ng==";
        const string databaseName = "sampledb";
        const string containerName = "orders";

        static void Main(string[] args)
        {
            //Order order = new Order()
            //{
            //    Id=Guid.NewGuid().ToString(),
            //    CustomerName="Kishore",
            //    Email="kishore@gmail.com",
            //    Amount=5000,
            //    OrderDate=DateTime.Now,
            //    OrderType="Online",
            //    Approved=true
            //};
            OrderService orderService = new OrderService(accountUri, accountKey, databaseName, containerName);
            //var result=orderService.InsertOrderAsync(order).GetAwaiter().GetResult();
            //Console.WriteLine($"Order created with Id {result.Id}");

            //var orders = orderService.GetOrdersAsync().GetAwaiter().GetResult();
            //foreach(var order in orders)
            //{
            //    Console.WriteLine($"{order.CustomerName}  -  {order.Amount}");
            //}

            string filePath = @"..\..\..\spCreateOrder.js";
            orderService.CreateProcedureAsync(databaseName, containerName, "spCreateOrder", filePath);
            Console.WriteLine("Procedure created");

            Order order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                CustomerName = "Dervin",
                Email = "dervin@gmail.com",
                Amount = 5000,
                OrderDate = DateTime.Now,
                OrderType = "Online",
                Approved = true
            };
            orderService.ExecuteProcedureAsync(databaseName, containerName, "spCreateOrder", order);
            Console.WriteLine("Inserted");

            Console.ReadKey();
        }
    }
}
