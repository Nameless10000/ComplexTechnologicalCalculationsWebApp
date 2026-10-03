import json
import grpc

from generated import calculation_pb2
from generated import calculation_pb2_grpc

from app.calculation import calculate


class FurnaceCalculationGrpcService(
    calculation_pb2_grpc.FurnaceServiceServicer
):
    def Calculate(self, request, context):
        try:
            data = json.loads(request.json)
        except json.JSONDecodeError:
            context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                "Invalid furnace request payload."
            )

        try:
            result = calculate(data)

            return calculation_pb2.CalculationReply(
                json=json.dumps(
                    result,
                    ensure_ascii=False
                )
            )

        except TypeError as exception:
            context.abort(
                grpc.StatusCode.INVALID_ARGUMENT,
                str(exception)
            )

        except Exception as exception:
            context.abort(
                grpc.StatusCode.INTERNAL,
                str(exception)
            )