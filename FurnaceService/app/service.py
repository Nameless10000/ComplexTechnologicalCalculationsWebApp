import json
import logging
import grpc
from generated import calculation_pb2, calculation_pb2_grpc
from app.calculation import calculate


class FurnaceCalculationGrpcService(calculation_pb2_grpc.FurnaceServiceServicer):
    def Calculate(self, request, context):
        reply = calculation_pb2.CalculationReply(request_id=request.request_id, module="furnace", correlation_id=request.correlation_id)
        try:
            if request.module and request.module != "furnace":
                raise ValueError("Модуль запроса не совпадает с сервисом.")
            data = json.loads(request.json, parse_constant=lambda value: (_ for _ in ()).throw(ValueError(f"Недопустимое число: {value}")))
            reply.json = json.dumps(calculate(data), ensure_ascii=False, allow_nan=False)
            reply.status = "Succeeded"
        except (ValueError, TypeError) as error:
            reply.status = "Failed"
            reply.error_code = "CALCULATION_VALIDATION_ERROR"
            reply.error_message = str(error)
            if not request.request_id:
                context.abort(grpc.StatusCode.INVALID_ARGUMENT, reply.error_message)
        except ArithmeticError:
            reply.status = "Failed"
            reply.error_code = "CALCULATION_ERROR"
            reply.error_message = "Для этих параметров формулы не определены. Проверьте делители."
            if not request.request_id:
                context.abort(grpc.StatusCode.FAILED_PRECONDITION, reply.error_message)
        except Exception:
            logging.exception("Furnace calculation failed; request_id=%s", request.request_id)
            reply.status = "Failed"
            reply.error_code = "CALCULATION_ERROR"
            reply.error_message = "Расчёт не удалось выполнить."
            if not request.request_id:
                context.abort(grpc.StatusCode.INTERNAL, reply.error_message)
        return reply
