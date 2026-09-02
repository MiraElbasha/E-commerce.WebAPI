using E_Commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGatway
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount ,string currency, CancellationToken ct = default);
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId , decimal amount , CancellationToken ct = default);
        
    }
}
