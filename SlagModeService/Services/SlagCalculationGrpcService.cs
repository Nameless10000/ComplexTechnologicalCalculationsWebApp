using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Contracts.Grpc;
using Grpc.Core;
using Newtonsoft.Json;

namespace SlagModeService.Services;

public sealed class SlagCalculationGrpcService(SlagMode library, IConfiguration configuration)
    : SlagCalculator.SlagCalculatorBase
{
    public override Task<CalculationReply> Calculate(CalculationRequest request, ServerCallContext context)
    {
        var model = JsonConvert.DeserializeObject<RequestData>(request.Json)
                    ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid slag request payload."));

        model.User = new UserAuthData
        {
            UserName = configuration["Authorization:UserName"],
            Password = configuration["Authorization:Password"]
        };

        var response = library.Calculate(model);

        return Task.FromResult(new CalculationReply
        {
            Json = JsonConvert.SerializeObject(response)
        });
    }
}
