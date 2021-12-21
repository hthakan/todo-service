using MongoDB.Bson.Serialization.Attributes;
using MongoDBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace todo_service.Models.OrderModels
{
    [BsonCollection("Orders")]
    public class Order : MongoDocument
    {
        public string BuyerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrderDetail OrderDetail { get; set; }
        [JsonIgnore]
        public int StatusId { get; set; }

        [BsonIgnore]
        public string OrderStatus { get; set; }

    }
}
