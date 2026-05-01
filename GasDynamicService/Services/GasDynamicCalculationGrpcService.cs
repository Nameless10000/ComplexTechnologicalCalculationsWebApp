using BaseLib;
using BaseLib.Models2;
using Contracts.Grpc;
using Grpc.Core;
using Newtonsoft.Json;

namespace GasDynamicService.Services;

public sealed class GasDynamicCalculationGrpcService(BlastFurnaceSmeltingGasDynamicModeXLLibrary library)
    : GasDynamicCalculator.GasDynamicCalculatorBase
{
    public override Task<CalculationReply> Calculate(CalculationRequest request, ServerCallContext context)
    {
        var model = JsonConvert.DeserializeObject<RequestModelV2>(request.Json)
                    ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid gas dynamic request payload."));

        var response = library.Calculate(model);

        return Task.FromResult(new CalculationReply
        {
            Json = JsonConvert.SerializeObject(response)
        });
    }
}
