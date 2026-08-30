using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class BasketService(IBasketRepository basketRepository , IMapper mapper) : IBasketService
    {
        public async Task<Result<BasketDTO>> CreateOrUpdateBasketAsync(BasketDTO basket, CancellationToken ct = default)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            var basketResult = await basketRepository.CreateOrUpdateBasketAsync(customerBasket, ct: ct);
            return basketResult != null ? Result<BasketDTO>.Ok(mapper.Map<BasketDTO>(basketResult)) : Result<BasketDTO>.Fail(Error.Failure("Can not Create Or Update Basket"));
        }

        public async Task<Result<bool>> DeleteBasketAsync(string Id, CancellationToken ct = default)
        {
            var result = await basketRepository.DeleteBasketAsync(Id, ct);
            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Can Not Delete Basket"));
        }

        public async Task<Result<BasketDTO>> GetBasketAsync(string Id, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(Id, ct);
            if(basket == null)
                return Result<BasketDTO>.Fail(Error.NotFound("Basket Not Found"));
           return mapper.Map<BasketDTO>(basket);
        }
    }
}
