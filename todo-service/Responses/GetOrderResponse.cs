using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using todo_service.Models.OrderModels;

namespace todo_service.Responses
{
    public class GetOrderResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public string OrderStatus { get; set; }
    }
}
