using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order = todo_service.Models.OrderModels;


namespace EventBusMessages.Messages
{
    public class OrderCreatedIntegrationEvent : Event
    {
        public Order::Order OrderItem { get; set; }

        public OrderCreatedIntegrationEvent(Order::Order order)
        {
            OrderItem = order;
        }
    }

}
