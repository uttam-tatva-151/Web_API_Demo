using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web_API.Attributes;

 /// <summary>
    /// Action filter that validates ModelState and short-circuits the request with a 400
    /// ValidationProblemDetails response when the model is invalid.
    /// Apply globally, per-controller, or put this attribute on a base controller to cover all controllers.
    /// </summary>
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ValidationProblemDetails details = new(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred."
                };

                context.Result = new BadRequestObjectResult(details);
            }
        }
    }
