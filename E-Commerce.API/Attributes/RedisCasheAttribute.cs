using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCasheAttribute :ActionFilterAttribute
    {
        private readonly int _durationInSec;

        public RedisCasheAttribute(int durationInSec = 90)
        {
            _durationInSec = durationInSec;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Get Cshe Service from Container (not injected directly in constructor)
            var casheService = context.HttpContext.RequestServices.GetRequiredService<ICasheService>();

            var casheKey = CreateCasheKey(context.HttpContext.Request);

            var cashed = await casheService.GetAsync(casheKey);
            //if data exists InCashe => Get From Cashe And Skip EndPoint
            if (!string.IsNullOrEmpty(cashed))
            {
                context.Result = new ContentResult
                {
                    Content = cashed,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            //If data not exists in cashe => excute endpoint and store results in cashe if results is ok
            var excuted = await next.Invoke();
            if (excuted.Result is OkObjectResult { Value : not null} ok)
            {
                await casheService.SetAsync(casheKey, ok.Value, TimeSpan
                   .FromSeconds(_durationInSec));
            }
        }

        private string CreateCasheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path).Append('?');
            foreach (var (k, v) in request.Query.OrderBy(q => q.Key)) 
                key.Append(k).Append('=').Append(v).Append('&');
            return key.ToString();
        }
    }
}
