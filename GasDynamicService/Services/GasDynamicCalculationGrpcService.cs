using BaseLib;
using BaseLib.Models2;
using Contracts.Grpc;
using Grpc.Core;
using Newtonsoft.Json;

namespace GasDynamicService.Services;

public sealed class GasDynamicCalculationGrpcService(BlastFurnaceSmeltingGasDynamicModeXLLibrary library)
    : GasDynamicCalculator.GasDynamicCalculatorBase
{
    public override Task<HealthReply> CheckHealth(HealthRequest request, ServerCallContext context) => Task.FromResult(new HealthReply { Status = "Serving", Module = "gas-dynamic" });

    public override Task<CalculationReply> Calculate(CalculationRequest request, ServerCallContext context)
    {
        return Task.FromResult(CalculationRpc.Run(request, "gas-dynamic", () =>
        {
            var model = JsonConvert.DeserializeObject<RequestModelV2>(request.Json)!;
            return JsonConvert.SerializeObject(library.Calculate(model));
        }));
    }
}
