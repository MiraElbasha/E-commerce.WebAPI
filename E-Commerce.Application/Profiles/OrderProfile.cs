using AutoMapper;
using E_Commerce.Application.DTOs.Authentication;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAddress , AddressDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.GetTotal()));

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();
                
               

        }
    }
}
