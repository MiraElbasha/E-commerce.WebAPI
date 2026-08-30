using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

    public class BasketController(IBasketService basketService) : APIBaseController
    {
        #region Get Basket
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<BasketDTO>> Getbasket(string id, CancellationToken ct = default)
        {
            var basket = await basketService.GetBasketAsync(id, ct);
            return ToActionResult(basket);
        }
        #endregion

        #region Create Or Update
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdate(BasketDTO basketDTO, CancellationToken ct = default)
        {
            var saved = await basketService.CreateOrUpdateBasketAsync(basketDTO, ct);
            return ToActionResult(saved);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct = default)
        
        {
            var result = await basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
        #endregion
    }
}
