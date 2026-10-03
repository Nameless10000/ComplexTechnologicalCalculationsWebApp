using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Contracts.Grpc;
using Grpc.Core;
using Newtonsoft.Json;

namespace SlagModeService.Services;

public sealed class SlagCalculationGrpcService(SlagMode library, IConfiguration configuration)
    : SlagCalculator.SlagCalculatorBase
{
    public override Task<HealthReply> CheckHealth(HealthRequest request, ServerCallContext context) => Task.FromResult(new HealthReply { Status = "Serving", Module = "slag-mode" });

    public override Task<CalculationReply> Calculate(CalculationRequest request, ServerCallContext context)
    {
        return Task.FromResult(CalculationRpc.Run(request, "slag-mode", () =>
        {
            var model = JsonConvert.DeserializeObject<RequestData>(request.Json)!;
            model.User = new UserAuthData { UserName = configuration["Authorization:UserName"]!, Password = configuration["Authorization:Password"]! };
            return JsonConvert.SerializeObject(library.Calculate(model));
        }));
    }
}
