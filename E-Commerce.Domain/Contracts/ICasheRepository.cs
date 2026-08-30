using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ICasheRepository
    {
        Task<string?> GetAsync(string casheKey, CancellationToken ct = default);
        Task SetAsync(string CashKey, string CasheValue, TimeSpan TimeToLive, CancellationToken ct = default);

    }
}
