using E_Commerce.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Order
{
    public class OrderToReturnDTO
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; } = [];
        public AddressDTO ShipToAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; } = default!;

        public decimal SubTotal { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Total { get; set; }
    }
}
