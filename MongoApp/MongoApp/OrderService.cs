using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoApp
{
    public class OrderService
    {
        MongoClient mongoClient;
        IMongoDatabase database;
        IMongoCollection<Order> orders;

        public OrderService(string connectionString, string dbName)
        {
            mongoClient = new MongoClient(connectionString);
            database = mongoClient.GetDatabase(dbName);
            orders = database.GetCollection<Order>("orders");
        }

        public async Task<Order> InsertOrderAsync(Order order)
        {
            await orders.InsertOneAsync(order);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await orders.Find(order=>true).ToListAsync();
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await orders.Find(order => order.Id==id).FirstOrDefaultAsync();
        }
    }
}
