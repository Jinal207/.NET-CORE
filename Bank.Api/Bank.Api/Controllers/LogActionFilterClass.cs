using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LogActionFilterClass : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Debug.WriteLine($"[Before] Action: {context.ActionDescriptor.DisplayName}");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Debug.WriteLine($"[After] Action: {context.ActionDescriptor.DisplayName}");
    }
}
