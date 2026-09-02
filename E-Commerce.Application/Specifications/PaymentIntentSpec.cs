using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class PaymentIntentSpec : BaseSpecifications<Order , Guid>
    {
        public PaymentIntentSpec(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
