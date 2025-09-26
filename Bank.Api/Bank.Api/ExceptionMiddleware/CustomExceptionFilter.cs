using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.ExceptionMiddleware
{
    public class CustomExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new { error = context.Exception.Message })
            {
                StatusCode = 500
            };
        }
    }
}
