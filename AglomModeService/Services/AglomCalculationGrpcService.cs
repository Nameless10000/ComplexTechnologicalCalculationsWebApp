using BaseLib.AglomMode;
using BaseLib.AglomMode.Models;
using Contracts.Grpc;
using Grpc.Core;
using Newtonsoft.Json;

namespace AglomModeService.Services;

public sealed class AglomCalculationGrpcService(AglomMode library)
    : AglomCalculator.AglomCalculatorBase
{
    public override Task<CalculationReply> Calculate(CalculationRequest request, ServerCallContext context)
    {
        var model = JsonConvert.DeserializeObject<AglomRequestData>(request.Json)
                    ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid aglom request payload."));

        var response = library.Calculate(model);

        return Task.FromResult(new CalculationReply
        {
            Json = JsonConvert.SerializeObject(response)
        });
    }
}
