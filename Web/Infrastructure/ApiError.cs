using System.Diagnostics;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Web.Infrastructure;

public sealed record ApiError(string Code, string Message, object? Details, string TraceId)
{
    public static ApiError Create(HttpContext context, string code, string message, object? details = null) =>
        new(code, message, details, Activity.Current?.Id ?? context.TraceIdentifier);

    public static (int Status, string Code, string Message) Map(Exception exception) => exception switch
    {
        Data.Infrastructure.ConflictException => (409, "CONFLICT", exception.Message),
        RpcException { StatusCode: StatusCode.InvalidArgument } rpc => (400, "CALCULATION_VALIDATION_ERROR", rpc.Status.Detail),
        RpcException { StatusCode: StatusCode.NotFound } => (404, "NOT_FOUND", "Расчёт не найден."),
        RpcException { StatusCode: StatusCode.DeadlineExceeded or StatusCode.Unavailable } =>
            (503, "SERVICE_UNAVAILABLE", "Расчётный сервис временно недоступен."),
        RpcException => (422, "CALCULATION_ERROR", "Расчёт не удалось выполнить для заданных параметров."),
        ArgumentException => (400, "VALIDATION_ERROR", exception.Message),
        KeyNotFoundException => (404, "NOT_FOUND", "Запись не найдена."),
        UnauthorizedAccessException => (403, "FORBIDDEN", "Нет доступа к записи."),
        DbUpdateException => (409, "CONFLICT", "Не удалось сохранить запись. Проверьте уникальность имени."),
        HttpRequestException or TimeoutException => (503, "SERVICE_UNAVAILABLE", "Внешний сервис временно недоступен."),
        _ => (500, "INTERNAL_ERROR", "Произошла внутренняя ошибка. Обратитесь к администратору с traceId.")
    };
}
