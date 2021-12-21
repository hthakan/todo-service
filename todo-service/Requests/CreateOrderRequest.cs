using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using todo_service.Models.OrderModels;

namespace todo_service.Requests
{
    public class CreateOrderRequest
    {

        /// <summary>
        /// BuyerId of Order
        /// </summary>
        [Required]
        public string BuyerId { get; set; }

        /// <summary>
        /// Name of Order
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// Description of Order
        /// </summary>
        [Required]
        public string Description { get; set; }


        /// <summary>
        /// Details of Order
        /// </summary>
        [Required]
        public OrderDetail OrderDetail { get; set; }

    }

    #region Example

    public class CreateOrderRequestExample : IExamplesProvider<CreateOrderRequest>
    {
        public CreateOrderRequest GetExamples()
        {
            return new CreateOrderRequest
            {
                BuyerId = "4574765857867",
                Name = "Sema Kudu",
                Description = "Order 1",
                OrderDetail = new OrderDetail { CategoryId = 2, ImageURL = "imageurl", Price = 6.8 }
            };
        }
    }

    #endregion
}
