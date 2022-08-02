using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ASP.NET.Core.WebAPI.Models.DTOs;

namespace ASP.NET.Core.WebAPI.Infrastructure.API.Filters;

public class GlobalErrorResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult)
        {
            var result = context.Result as ObjectResult;
            if (result.StatusCode.HasValue && result.StatusCode.Value >= StatusCodes.Status400BadRequest && result.Value is not null && result.Value is ProblemDetails)
            {
                // Since all controllers are decorated with ApiController attribute, any request to server that fails Model Binding will be taken care
                // of automatically by ASP.NET Core, however if any valid request is made which later becomes invalid (example HTTP PATCH API) then 
                // the invalid model state will be processed and information will be extracted here to respond to the client.

                ErrorResponse error = new(result.Value as ProblemDetails, context.ModelState);
                context.Result = new ObjectResult(error)
                {
                    StatusCode = result.StatusCode
                };
            }
        }
    }
}
