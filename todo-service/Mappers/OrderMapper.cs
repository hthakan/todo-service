//using AutoMapper;
//using System;
//using todo_service.Enums;
//using todo_service.Models.OrderModels;
//using todo_service.Requests;
//using todo_service.Responses;

//namespace todo_service.Mappers
//{
//    public class OrderMapper : Profile
//    {
//        public OrderMapper()
//        {
//            #region requests
//            // Default mapping when property names are same
//            //CreateMap<CreateOrderRequest, Order>();

//            // Mapping when property names are different

//            //create order
//            CreateMap<CreateOrderRequest, Order>()
//                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)OrderStatus.Initial))
//                .AfterMap((src, dest) => dest.OrderDetail.ReferenceNumber = "Eshop Reference");


//            #endregion

//            #region responses
//            //FOR RESPONSES
//            // Default mapping when property names are same
//            //CreateMap<CreateOrderOUT, CreateOrderResponse>();

//            // Mapping when property names are different


//            //get order
//            CreateMap<Order, GetOrderResponse>()
//                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
//                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => Enum.GetName(typeof(OrderStatus), src.StatusId)));

//            CreateMap<OrderDetail, OrderDetail>();

//            #endregion

//        }
//    }
//}
