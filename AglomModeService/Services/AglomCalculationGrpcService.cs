using BaseLib.AglomMode;
using BaseLib.AglomMode.Models;
using Contracts.Grpc;
using Grpc.Core;
using Newtonsoft.Json;

namespace AglomModeService.Services;

public sealed class AglomCalculationGrpcService(AglomMode library)
    : AglomCalculator.AglomCalculatorBase
{
    public override Task<HealthReply> CheckHealth(HealthRequest request, ServerCallContext context) => Task.FromResult(new HealthReply { Status = "Serving", Module = "aglom-mode" });

    public override Task<CalculationReply> Calculate(CalculationRequest request, ServerCallContext context)
    {
        return Task.FromResult(CalculationRpc.Run(request, "aglom-mode", () =>
        {
            var model = JsonConvert.DeserializeObject<AglomRequestData>(request.Json)!;
            return JsonConvert.SerializeObject(library.Calculate(model));
        }));
    }
}
