using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Web.Infrastructure;

namespace Test;

public class ApiErrorTest
{
    [Theory]
    [InlineData(StatusCode.InvalidArgument, 400, "CALCULATION_VALIDATION_ERROR")]
    [InlineData(StatusCode.NotFound, 404, "NOT_FOUND")]
    [InlineData(StatusCode.Unavailable, 503, "SERVICE_UNAVAILABLE")]
    [InlineData(StatusCode.Internal, 422, "CALCULATION_ERROR")]
    public void MapsGrpcErrors(StatusCode grpcStatus, int httpStatus, string code)
    {
        var mapped = ApiError.Map(new RpcException(new Status(grpcStatus, "test")));
        Assert.Equal(httpStatus, mapped.Status);
        Assert.Equal(code, mapped.Code);
    }

    [Fact]
    public void UnexpectedErrorDoesNotExposeInternalMessage()
    {
        var mapped = ApiError.Map(new Exception("connection-password"));
        Assert.Equal(500, mapped.Status);
        Assert.DoesNotContain("connection-password", mapped.Message);
        Assert.False(string.IsNullOrWhiteSpace(ApiError.Create(new DefaultHttpContext(), mapped.Code, mapped.Message).TraceId));
    }
}
