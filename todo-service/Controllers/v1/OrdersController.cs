//using AutoMapper;
//using ErrorHandler.Models;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Swashbuckle.AspNetCore.Filters;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;
//using todo_service.Requests;
//using todo_service.Responses;

//namespace todo_service.Controllers.v1
//{
//    //[Authorize]
//    [ApiVersion("1.0")]
//    [Route("api/[controller]")]
//    [ApiController]
//    [Produces("application/json")]
//    public class OrdersController : ControllerBase
//    {
//        private readonly IOrderOperations _orderOperations;
//        private readonly ILogger<OrdersController> _logger;
//        private readonly IMapper _mapper;

//        public OrdersController(IOrderOperations orderOperations, ILogger<OrdersController> logger, IMapper mapper)
//        {
//            _orderOperations = orderOperations ?? throw new ArgumentNullException(nameof(orderOperations));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            _mapper = mapper;
//        }


//        /// <summary>
//        /// Get Order By Id
//        /// </summary>
//        ///<remarks>
//        /// Gets Order with relevant ID. Your controller description goes here.
//        ///</remarks>
//        ///<param name="orderId">gets order according to given orderId</param>
//        ///<response code="400">Validation Error</response>
//        ///<response code="401">Unauthorized</response>
//        /// <response code="404">Order not found</response>
//        [Route("{orderId}")]
//        [HttpGet]
//        [ProducesResponseType(typeof(GetOrderResponse), (int)HttpStatusCode.OK)]
//        [ProducesResponseType((int)HttpStatusCode.NotFound)]
//        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
//        public async Task<ActionResult> GetOrderAsync(string orderId)
//        {
//            try
//            {
//                var order = await _orderOperations.GetOrder(orderId);

//                if (order == null)
//                    return NotFound();

//                _logger.LogDebug("Order has selected {@order}", order);
//                GetOrderResponse response = _mapper.Map<GetOrderResponse>(order);

//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                throw new CustomException("2002", "Error", $"Exception: {ex.Message}");

//            }
//        }


//        /// <summary>
//        /// Create Order
//        /// </summary>
//        ///<remarks>
//        /// Creates a new order.
//        ///</remarks>
//        ///<response code="400">Validation Error</response>
//        ///<response code="401">Unauthorized</response>
//        /// <response code="500">Server Error</response>
//        [Route("")]
//        [HttpPost]
//        [SwaggerRequestExample(typeof(CreateOrderRequest), typeof(CreateOrderRequestExample))]
//        [ProducesResponseType((int)HttpStatusCode.OK)]
//        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
//        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
//        public async Task<ActionResult> CreateOrderAsync([FromBody] CreateOrderRequest createOrderRequest)
//        {
//            try
//            {
//                Order order = _mapper.Map<Order>(createOrderRequest);
//                await _orderOperations.CreateOrder(order);
//                _logger.LogDebug("order created", order);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                throw new CustomException("2002", "Error", $"Exception: {ex.Message}");
//            }

//        }


//        /// <summary>
//        /// Update Order Status
//        /// </summary>
//        ///<remarks>
//        /// Update a status of order with relevant orderId and status.
//        ///</remarks>
//        ///<response code="400">Validation Error</response>
//        ///<response code="401">Unauthorized</response>
//        /// <response code="500">Server Error</response>
//        [Route("{orderId}/approve-order")]
//        [HttpPut]
//        [ProducesResponseType((int)HttpStatusCode.OK)]
//        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
//        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
//        public async Task<IActionResult> ApproveOrderAsync(string orderId)
//        {
//            try
//            {
//                await _orderOperations.ApproveOrder(orderId);

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                throw new CustomException("2002", "Error", $"Exception: {ex.Message}");
//            }

//        }
//    }
//}
