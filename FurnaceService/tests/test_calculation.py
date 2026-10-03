import json
import math
import unittest
from pathlib import Path
from concurrent import futures
import grpc
from app.calculation import calculate
from app.service import FurnaceCalculationGrpcService
from generated import calculation_pb2, calculation_pb2_grpc


class CalculationTest(unittest.TestCase):
    def setUp(self):
        self.data = json.loads((Path(__file__).parent / "sample.json").read_text())

    def test_existing_sample_balance_closes(self):
        result = calculate(self.data)["heat_balance"]
        self.assertTrue(all(math.isfinite(value) for value in result.values()))
        self.assertAlmostEqual(result["C15"], result["C46"])
        self.assertAlmostEqual(result["C47"], 1)

    def test_invalid_values(self):
        for field, value in (("rd", 0), ("gas_consumption", 0), ("slag_rate", -1), ("Si", float("nan")), ("hot_blast_temp", True), ("C", 101)):
            with self.subTest(field=field):
                data = {**self.data, field: value}
                with self.assertRaises(ValueError):
                    calculate(data)

    def test_grpc_metadata_and_legacy_client(self):
        server = grpc.server(futures.ThreadPoolExecutor(max_workers=1))
        calculation_pb2_grpc.add_FurnaceServiceServicer_to_server(FurnaceCalculationGrpcService(), server)
        port = server.add_insecure_port("127.0.0.1:0")
        server.start()
        try:
            with grpc.insecure_channel(f"127.0.0.1:{port}") as channel:
                client = calculation_pb2_grpc.FurnaceServiceStub(channel)
                health = client.CheckHealth(calculation_pb2.HealthRequest(), timeout=5)
                self.assertEqual((health.status, health.module), ("Serving", "furnace"))
                reply = client.Calculate(calculation_pb2.CalculationRequest(json=json.dumps(self.data), request_id="req", correlation_id="cor", module="furnace"), timeout=5)
                self.assertEqual((reply.status, reply.request_id, reply.correlation_id), ("Succeeded", "req", "cor"))
                reply = client.Calculate(calculation_pb2.CalculationRequest(json="{}", request_id="bad"), timeout=5)
                self.assertEqual(reply.error_code, "CALCULATION_VALIDATION_ERROR")
                with self.assertRaises(grpc.RpcError) as error:
                    client.Calculate(calculation_pb2.CalculationRequest(json="{}"), timeout=5)
                self.assertEqual(error.exception.code(), grpc.StatusCode.INVALID_ARGUMENT)
        finally:
            server.stop(0).wait()
