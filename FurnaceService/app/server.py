from concurrent import futures

import grpc

from generated import calculation_pb2_grpc
from app.service import FurnaceCalculationGrpcService


def serve():
    server = grpc.server(
        futures.ThreadPoolExecutor(
            max_workers=10
        )
    )

    calculation_pb2_grpc.add_FurnaceServiceServicer_to_server(
        FurnaceCalculationGrpcService(),
        server
    )

    server.add_insecure_port(
        "[::]:8080"
    )

    print(
        "FurnaceService started on port 8080",
        flush=True
    )

    server.start()
    server.wait_for_termination()


if __name__ == "__main__":
    serve()