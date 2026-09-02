using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Create Order
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO , CancellationToken ct )
       => ToActionResult(await _orderService.CreateOrderAsync(orderDTO,GetEmailFromToken(), ct));
        #endregion

        #region Get All Delivery Methods
        [AllowAnonymous]
        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTO>>> GetAllDeliveryMethods(CancellationToken ct)
        => ToActionResult(await _orderService.GetAllDeliveryMethodsAsync(ct));
        #endregion

        #region Get All Orders
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetAllOrders(CancellationToken ct)
            => ToActionResult(await _orderService.GetAllOrdersAsync(GetEmailFromToken(), ct));
        #endregion

        #region Get Order By Id And Email
        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdAndEmail(Guid id, CancellationToken ct)
            => ToActionResult(await _orderService.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), ct));

        #endregion
    }
}
