using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosSQLApiDemo
{
    public class Order
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }
    }
}
