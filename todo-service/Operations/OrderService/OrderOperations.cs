using Common.Services;
using EventBus;
using EventBusMessages.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todo_service.Enums;
using todo_service.Models.OrderModels;

namespace todo_service.OrderService
{
    public class OrderOperations : IOrderOperations
    {
        private readonly IMongoDbBaseRepository<Order> _orderRepository;
        private readonly ILogger _logger;
        private readonly IIdentityService _identityService;
        private readonly IEventBusManager _eventBusManager;


        public OrderOperations(IMongoDbBaseRepository<Order> orderRepository, ILogger<OrderOperations> logger, IIdentityService identityService, IEventBusManager eventBusManager)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger;
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _eventBusManager = eventBusManager ?? throw new ArgumentNullException(nameof(eventBusManager));
        }


        //Bussiness logic goes here
        public async Task<Order> GetOrder(string id)
        {
            return await _orderRepository.FindByIdAsync(id);

        }

        public async Task<List<Order>> GetOrders()
        {
            return await _orderRepository.FindAsync(p => p.StatusId == (int)OrderStatus.Initial);
            //throw new CustomException("3002", "Database Error", $"Exception: {ex.Message}", true);
            //throw new CustomException("26153", "Validations Error", null, (int)HttpStatusCode.NotFound);
        }

        public async Task CreateOrder(Order order)
        {
            await _orderRepository.InsertOneAsync(order);
            await _eventBusManager.PublishAsync(new OrderCreatedIntegrationEvent(order));
        }

        public async Task ApproveOrder(string OrderId)
        {
            var order = await _orderRepository.FindByIdAsync(OrderId);

            order.StatusId = (int)OrderStatus.Approved;
            await _orderRepository.ReplaceOneAsync(order);
        }
    }
}
