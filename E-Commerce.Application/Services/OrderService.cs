using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Specifications;
using E_Commerce.Application.DTOs.Baskets;

namespace E_Commerce.Application.Services
{
    public class OrderService(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository) : IOrderService
    {
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(orderDTO.BasketId, ct);
           if (basket == null)
                return Result<OrderToReturnDTO>.Fail(Error.NotFound("Basket not found" , $"BasketWithId {orderDTO.BasketId} Is Not Found"));
            if (basket.Items.Count == 0)
                return Result<OrderToReturnDTO>.Fail(Error.Validation("Basket is empty", $"Can Not Create Order For Empty Basket with Id {orderDTO.BasketId}"));


            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var productRepo = unitOfWork.GetRepository<Product, int>();

            var productIds = basket.Items.Select(i => i.Id).ToHashSet();
            var products = (await productRepo.GetAllAsync(new ProductWithIdsSpecifications(productIds), ct)).ToDictionary(x => x.Id);
            var orderItems = new List<OrderItem>(basket.Items.Count);
            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                {
                    return Result<OrderToReturnDTO>.Fail(Error.NotFound("Product not found", $"Product with Id {item.Id} is not found"));
                }

                orderItems.Add(new OrderItem
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdered
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    }
                });
            }

            var orderAddress = mapper.Map<OrderAddress>(orderDTO.ShipToAddress);
            var deliveryRepo = unitOfWork.GetRepository<DeliveryMethod, int>();
            var deliveryMethod = await deliveryRepo.GetByIdAsync(orderDTO.DeliveryMethodId, ct);
            if (deliveryMethod == null) return Result<OrderToReturnDTO>.Fail(Error.NotFound("Delivery method not found", $"Delivery method with Id {orderDTO.DeliveryMethodId} is not found"));
            var subTotal = orderItems.Sum(x => x.Quantity * x.Price);
            var order = new Order(email, orderItems, orderAddress, deliveryMethod, subTotal);

            orderRepo.Add(order);
            var result = await unitOfWork.SaveChangesAsync(ct);
            if (result <= 0) return Result<OrderToReturnDTO>.Fail(Error.Failure("Order Save failed", "Can't Create order"));
            await basketRepository.DeleteBasketAsync(orderDTO.BasketId , ct);
            return mapper.Map<OrderToReturnDTO>(order);
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct: ct);
            return Result<IReadOnlyList<DeliveryMethodDTO>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDTO>>(deliveryMethods));
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDTO>>> GetAllOrdersAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecifications(email) , ct);
            return Result<IReadOnlyList<OrderToReturnDTO>>.Ok(mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAndEmailAsync(Guid Id, string email, CancellationToken ct = default)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(Id, email), ct);
            if (order == null) return Result<OrderToReturnDTO>.Fail(Error.NotFound("Order not found", $"Order with Id {Id} is not found"));
            return Result<OrderToReturnDTO>.Ok(mapper.Map<OrderToReturnDTO>(order));
        }
    }
}
