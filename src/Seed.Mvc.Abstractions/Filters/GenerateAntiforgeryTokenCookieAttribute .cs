using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Seed.Mvc.Filters
{
    public class GenerateAntiforgeryTokenCookieAttribute : ActionFilterAttribute
    {
        public const string CookieName = "XSRF-TOKEN";

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();
            var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);

            context.HttpContext.Response.Cookies.Append(
                CookieName,
                tokens.RequestToken,
                new CookieOptions() { HttpOnly = false });
        }
    }
}