using AutoMapper;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    public class OrderItemPictureUrlResolver(IOptions<UrlSettings> options) : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly UrlSettings _urlSettings = options.Value;
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl)) 
            {
                return string.Empty;
            }
            return $"{_urlSettings.BaseUrl}{source.Product.PictureUrl}";
        }
    }

}
