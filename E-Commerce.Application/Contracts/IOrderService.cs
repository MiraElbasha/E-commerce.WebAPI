using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email, CancellationToken ct = default);

        Task<Result<IReadOnlyList<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDTO>>> GetAllOrdersAsync(string email ,CancellationToken ct = default);
        Task<Result<OrderToReturnDTO>> GetOrderByIdAndEmailAsync(Guid Id, string email, CancellationToken ct = default);
        
    }
}
