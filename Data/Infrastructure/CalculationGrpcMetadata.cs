using System.Diagnostics;
using Contracts.Grpc;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace Data.Infrastructure;

public static class CalculationGrpcMetadata
{
    public static CalculationRequest Request(string json, string module, HttpContext? context) => new()
    {
        Json = json, Module = module, RequestId = context?.TraceIdentifier ?? Guid.NewGuid().ToString("N"),
        CorrelationId = Activity.Current?.TraceId.ToString() ?? context?.TraceIdentifier ?? Guid.NewGuid().ToString("N")
    };

    public static void EnsureSuccess(CalculationReply reply)
    {
        if (reply.Status != "Failed") return; // Legacy servers omit metadata.
        var status = reply.ErrorCode switch
        { "CALCULATION_VALIDATION_ERROR" => StatusCode.InvalidArgument, "SERVICE_UNAVAILABLE" => StatusCode.Unavailable, _ => StatusCode.FailedPrecondition };
        throw new RpcException(new Status(status, reply.ErrorMessage));
    }
}
