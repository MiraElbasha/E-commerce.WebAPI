using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{

    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentGatwaySettings _stripeSettings;

        public PaymentController(IPaymentService paymentService , IOptions<PaymentGatwaySettings> options)
        {
            _paymentService = paymentService;
            _stripeSettings = options.Value;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        [ProducesResponseType(typeof(BasketDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePayment(string basketId, CancellationToken ct = default)
       => ToActionResult(await _paymentService.CreateOrUpadatePaymentIntentAsync(basketId, ct));


        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeSettings.WebHookSecret);

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:

                        var succeededPaymentIntent = stripeEvent.Data.Object as PaymentIntent;

                        if (succeededPaymentIntent is not null)
                            await _paymentService.PaymentSucceeded(succeededPaymentIntent.Id);

                        break;

                    case EventTypes.PaymentIntentPaymentFailed:

                        var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;

                        if (failedPaymentIntent is not null)
                            await _paymentService.PaymentFailed(failedPaymentIntent.Id);

                        break;

                    default:
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
