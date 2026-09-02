using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class PaymentGatwaySettings
    {
        public string SecretKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = "USD";

        public string WebHookSecret { get; set; } = default!;
    }
}
