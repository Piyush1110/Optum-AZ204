using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosSQLApiDemo
{
    public class OrderService
    {
        CosmosClient cosmosClient;
        Database database;
        Container container;

        public OrderService(string uri, string key, string databaseName, string collectionName)
        {
            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(uri, key);
            cosmosClient = clientBuilder
                                .WithConnectionModeDirect()
                                .Build();          
            cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName, 1000).GetAwaiter().GetResult();
            database=cosmosClient.GetDatabase(databaseName);
            database.CreateContainerIfNotExistsAsync(collectionName, "/orderType").GetAwaiter().GetResult();
            container=cosmosClient.GetContainer(databaseName,collectionName);
        }

        public async Task<Order> InsertOrderAsync(Order order)
        {
            var response=await container.CreateItemAsync<Order>(order, new PartitionKey(order.OrderType));
            return response.Resource;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var queryString = "Select * from c";
            var query = container.GetItemQueryIterator<Order>(new QueryDefinition(queryString));
            List<Order> results = new List<Order>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async void CreateProcedureAsync(string databaseName, string collectionName,string storedProcedureId, string procedureFilePath)
        {
            var body = File.ReadAllText(procedureFilePath);
            StoredProcedureResponse spResponse = await cosmosClient.GetContainer(databaseName, collectionName)
                .Scripts
                .CreateStoredProcedureAsync(new StoredProcedureProperties
                {
                    Id = storedProcedureId,
                    Body = body
                }); ;            
        }

        public async void ExecuteProcedureAsync(string databaseName, string collectionName, string storedProcedureId, Order order)
        {
            dynamic[] items = new dynamic[]
            {
                order
            };
            var result = await cosmosClient.GetContainer(databaseName, collectionName)
              .Scripts
              .ExecuteStoredProcedureAsync<string>(storedProcedureId, new PartitionKey(order.OrderType), items);
        }
    }
}
