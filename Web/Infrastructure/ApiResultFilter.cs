using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Infrastructure;

public sealed class ApiResultFilter : IAsyncAlwaysRunResultFilter, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var details = context.ModelState.Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(x => x.Key, x => x.Value!.Errors.Select(e =>
                    string.IsNullOrEmpty(e.ErrorMessage) ? "Некорректное значение." : e.ErrorMessage).ToArray());
            context.Result = new BadRequestObjectResult(ApiError.Create(context.HttpContext,
                "VALIDATION_ERROR", "Проверьте входные данные.", details));
            return;
        }
        await next();
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { StatusCode: >= 400 } result && result.Value is not ApiError)
            result.Value = ApiError.Create(context.HttpContext, "REQUEST_ERROR", "Не удалось выполнить запрос.", result.Value);
        await next();
    }
}
