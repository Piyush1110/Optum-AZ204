using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace MongoApp
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("customerName")]
        public string CustomerName { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("orderDate")]
        public DateTime OrderDate { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("approved")]
        public bool Approved { get; set; }
    }
}
