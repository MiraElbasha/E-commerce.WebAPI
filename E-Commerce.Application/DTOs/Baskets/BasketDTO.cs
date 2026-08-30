using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Baskets
{
    public class BasketDTO
    {
        public string Id { get; set; } = default!;

        public ICollection<BasketItemDTO> Items { get; set; } = [];
    }
}
