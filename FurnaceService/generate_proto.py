"""Regenerate Python stubs from the shared contract, from any working directory."""
from pathlib import Path
from grpc_tools import protoc

root = Path(__file__).resolve().parent
proto = root.parent / "Contracts" / "Protos"
target = root / "generated"
target.mkdir(exist_ok=True)
result = protoc.main(["protoc", f"-I{proto}", f"--python_out={target}", f"--grpc_python_out={target}", str(proto / "calculation.proto")])
if result:
    raise SystemExit(result)
stub = target / "calculation_pb2_grpc.py"
stub.write_text(stub.read_text().replace("import calculation_pb2 as", "from generated import calculation_pb2 as"))
