using Microsoft.AspNetCore.Mvc;
using BaseLib.Validation;
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
        if (string.Equals(context.RouteData.Values["action"]?.ToString(), "Calculate", StringComparison.OrdinalIgnoreCase))
        {
            var module = context.RouteData.Values["controller"]?.ToString() switch
            { "AglomMode" => "aglom-mode", "SlagMode" => "slag-mode", "GasDynamic" => "gas-dynamic", _ => null };
            if (module is not null)
            {
                context.HttpContext.Request.Body.Position = 0;
                using var reader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
                var json = await reader.ReadToEndAsync(context.HttpContext.RequestAborted);
                CalculationInputValidator.Validate(module, json);
            }
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
