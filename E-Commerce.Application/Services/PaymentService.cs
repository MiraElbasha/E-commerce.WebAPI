using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGatway _paymentGatway;
        private readonly PaymentGatwaySettings _stripeSettings;
        private readonly IMapper _mapper;

        public PaymentService(IBasketRepository basketRepository , IUnitOfWork unitOfWork, IPaymentGatway paymentGatway , IOptions<PaymentGatwaySettings> stripeSettings , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGatway = paymentGatway;
            _stripeSettings = stripeSettings.Value;
            _mapper = mapper;
        }

        public async Task<Result<BasketDTO>> CreateOrUpadatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId , ct);
            if (basket == null)
            {
                return Result<BasketDTO>.Fail(Error.NotFound("Basket Not Found", $"Basket With Id {basketId} Is Not Fpind"));
            }
            if (basket.Items.Count == 0)
            {
                return Result<BasketDTO>.Fail(Error.Validation("Basket Is Empty", $"Basket With Id {basketId} Is Empty"));
            }
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var productIds = basket.Items.Select(i => i.Id).ToHashSet();
            var products = (await productRepo.GetAllAsync(new ProductWithIdsSpecifications(productIds), ct )).ToDictionary(p => p.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                {
                    return Result<BasketDTO>.Fail(Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found"));
                }
                    item.Price = product.Price;
            }

            var deliveryRepo = _unitOfWork.GetRepository<DeliveryMethod, int>();
            var deliveryMethod = await deliveryRepo.GetByIdAsync(basket.DeliveryMethodId.Value , ct);

            if (deliveryMethod == null)
            {
                return Result<BasketDTO>.Fail(Error.NotFound("Delivery Method Not Found", $"Delivery Method With Id {basket.DeliveryMethodId.Value} Is Not Found"));
            }

            basket.ShippingPrice = deliveryMethod.Price;
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var amount = (long)Math.Round((subTotal + deliveryMethod.Price) + 100m);



            if (!string.IsNullOrEmpty(basket.PaymentIntentId) && basket.PaymentIntentId.StartsWith("pi_"))
            {
                await _paymentGatway.UpdatePaymentIntentAsync(basket.PaymentIntentId, amount, ct);
            }

            else 
            {
                var result = await _paymentGatway.CreatePaymentIntentAsync(amount, _stripeSettings.DefaultCurrency, ct);
                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket, ct:ct);
            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new PaymentIntentSpec(paymentIntentId));
            if (order == null) { return; }
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task PaymentSucceeded(string paymentIntentId)
        {
           var orderRepo = _unitOfWork.GetRepository<Order , Guid>();
            var order = await orderRepo.GetByIdAsync(new PaymentIntentSpec(paymentIntentId));
            if (order == null) { return; }
            order.Status = OrderStatus.PaymentReveived;
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
