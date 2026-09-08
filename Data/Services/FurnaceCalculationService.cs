using System.Security.Claims;
using Contracts.Grpc;
using Contracts.History;
using Microsoft.AspNetCore.Http;

namespace Data.Services;

public class FurnaceCalculationService(
    IHttpContextAccessor httpContextAccessor,
    FurnaceService.FurnaceServiceClient calculatorClient,
    CalculationHistoryProducerService historyProducer)
{
    private HttpContext? _httpContext => httpContextAccessor.HttpContext;

    private int _currentUserId =>
        int.Parse(
            _httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"
        );

    public async Task<string> Calculate(string requestJson)
    {
        var grpcResponse = await calculatorClient.CalculateAsync(
            new CalculationRequest
            {
                Json = requestJson
            });

        var responseJson = grpcResponse.Json;

        await historyProducer.PublishAsync(new CalculationHistoryEvent
        {
            Module = CalculationModules.Furnace,
            UserId = _currentUserId,
            CreationDateTime = DateTime.UtcNow,
            RequestJson = requestJson,
            ResponseJson = responseJson
        });

        return responseJson;
    }
}