using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class CasheService : ICasheService
    {
        private readonly ICasheRepository _casheRepository;

        public CasheService(ICasheRepository casheRepository)
        {
            _casheRepository = casheRepository;
        }
        public Task<string?> GetAsync(string casheKey, CancellationToken ct = default)
        =>  _casheRepository.GetAsync(casheKey, ct);

        public Task SetAsync(string casheKey, object casheValue, TimeSpan TimeToLive, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(casheValue, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return _casheRepository.SetAsync(casheKey, json, TimeToLive, ct);
        }
    }
}
