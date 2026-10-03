using BaseLib.Validation;
using Grpc.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Contracts.Grpc;

public static class CalculationRpc
{
    public static CalculationReply Run(CalculationRequest request, string module, Func<string> calculate)
    {
        var reply = new CalculationReply { RequestId = request.RequestId, CorrelationId = request.CorrelationId, Module = module };
        try
        {
            if (!string.IsNullOrEmpty(request.Module) && request.Module != module) throw new ArgumentException("Модуль запроса не совпадает с сервисом.");
            CalculationInputValidator.Validate(module, request.Json);
            reply.Json = calculate();
            var result = JToken.Parse(reply.Json);
            if (result is JContainer container && container.Descendants().OfType<JValue>()
                .Any(x => x.Value is double d && !double.IsFinite(d) || x.Value is string s && s is "NaN" or "Infinity" or "-Infinity"))
                throw new ArithmeticException("Результат содержит неконечные числа.");
            reply.Status = "Succeeded";
            return reply;
        }
        catch (Exception exception)
        {
            var validation = exception is ArgumentException or JsonException;
            var infrastructure = exception is HttpRequestException or TimeoutException;
            reply.Status = "Failed";
            reply.ErrorCode = validation ? "CALCULATION_VALIDATION_ERROR" : infrastructure ? "SERVICE_UNAVAILABLE" : "CALCULATION_ERROR";
            reply.ErrorMessage = exception is CalculationValidationException errors ? JsonConvert.SerializeObject(errors.Errors)
                : validation ? exception.Message : "Расчёт не удалось выполнить.";
            reply.Json = "";
            if (string.IsNullOrEmpty(request.RequestId))
                throw new RpcException(new Status(validation ? StatusCode.InvalidArgument : infrastructure ? StatusCode.Unavailable : StatusCode.FailedPrecondition, reply.ErrorMessage));
            return reply;
        }
    }
}
