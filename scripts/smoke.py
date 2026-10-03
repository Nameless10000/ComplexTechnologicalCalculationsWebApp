"""Integration check against an isolated running Compose project; no closed REST APIs.

python scripts/smoke.py --base-url http://localhost:3000/api
Add --compose-project NAME --kafka-outage only for a disposable test environment.
"""
import argparse
import http.cookiejar
import json
from pathlib import Path
import subprocess
import time
import urllib.error
import urllib.request
import uuid


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--base-url", default="http://localhost:3000/api")
    parser.add_argument("--compose-project")
    parser.add_argument("--kafka-outage", action="store_true")
    args = parser.parse_args()
    if args.kafka_outage and not args.compose_project:
        parser.error("--kafka-outage requires an explicitly named disposable Compose project")
    root = Path(__file__).resolve().parent.parent
    sample = json.loads((root / "FurnaceService/tests/sample.json").read_text())
    first = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))
    second = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()))

    def request(path, payload=None, method=None, expected=200, client=first, binary=False):
        req = urllib.request.Request(args.base_url + path, method=method,
            data=None if payload is None else json.dumps(payload).encode(),
            headers={"Content-Type": "application/json"})
        try:
            response = client.open(req, timeout=35)
        except urllib.error.HTTPError as error:
            response = error
        content = response.read()
        assert response.code == expected, (path, response.code, content[:600])
        return (content if binary or not content else json.loads(content)), response.headers

    def saved(identifier):
        deadline = time.monotonic() + 65
        while time.monotonic() < deadline:
            row, _ = request("/calculations/" + identifier)
            if row["status"] == "Saved":
                return row
            time.sleep(2)
        raise AssertionError("History did not reach Saved: " + identifier)

    def compose(*command):
        subprocess.run(["docker", "compose", "-p", args.compose_project, *command], cwd=root, check=True,
            stdout=subprocess.DEVNULL)

    error, _ = request("/calculations", expected=401)
    assert {"code", "message", "details", "traceId"} <= error.keys()
    stamp = uuid.uuid4().hex[:12]
    for client, suffix in ((first, "a"), (second, "b")):
        request("/Auth/SignUp", {"username": "smoke" + stamp + suffix,
            "email": stamp + suffix + "@example.test", "password": "SmokeCheck!42"}, client=client)
        request("/Auth/Me", client=client)
    health, _ = request("/health/ready")
    assert health["status"] == "Healthy", health
    result, headers = request("/Furnace/Calculate", sample)
    assert "heat_balance" in result
    left_id = headers["X-Calculation-Id"]
    assert headers["X-Correlation-Id"] and headers["X-History-Status"] == "Pending"
    left = saved(left_id)
    assert left["input"] == sample and left["output"] == result
    request("/calculations/" + left_id, expected=404, client=second)
    invalid = {**sample, "C": -1}
    error, _ = request("/Furnace/Calculate", invalid, expected=400)
    assert error["code"] == "CALCULATION_VALIDATION_ERROR" and error["traceId"]
    altered = {**sample, "coke_rate": sample["coke_rate"] + 1}
    _, headers = request("/Furnace/Calculate", altered)
    right_id = headers["X-Calculation-Id"]
    saved(right_id)
    comparison, _ = request(f"/calculations/compare?leftId={left_id}&rightId={right_id}")
    assert comparison["inputs"] and comparison["results"], comparison
    preset_input = {"module": "furnace", "name": "Проверка " + stamp, "description": "Интеграционный сценарий", "payload": sample}
    preset, _ = request("/presets", preset_input, expected=201)
    preset_id = preset["id"]
    request("/presets", preset_input, expected=409)
    request("/presets/" + preset_id, expected=404, client=second)
    preset_input["description"] = "Обновлено"
    updated, _ = request("/presets/" + preset_id, preset_input, method="PUT")
    assert updated["description"] == "Обновлено"
    presets, _ = request("/presets?module=furnace")
    assert any(item["id"] == preset_id for item in presets)
    for extension, signature in (("pdf", b"%PDF"), ("xlsx", b"PK")):
        exported, _ = request(f"/calculations/{left_id}/export?format={extension}", binary=True)
        assert exported.startswith(signature) and len(exported) > 1000
        request(f"/calculations/{left_id}/export?format={extension}", expected=404, client=second)
    request("/presets/" + preset_id, method="DELETE", expected=204)
    print("PASS: cookie auth, health, Furnace gRPC, validation, async history, ownership, presets, comparison, PDF/XLSX", flush=True)
    if args.kafka_outage:
        compose("stop", "kafka")
        try:
            health, _ = request("/health/ready")
            assert health["status"] == "Degraded", health
            _, headers = request("/Furnace/Calculate", sample)
            pending_id = headers["X-Calculation-Id"]
            time.sleep(8)  # Allow one broker timeout and a durable retry to be recorded.
            pending, _ = request("/calculations/" + pending_id)
            assert pending["status"] == "Pending"
        finally:
            compose("start", "kafka")
        saved(pending_id)
        print("PASS: Kafka outage keeps successful calculation Pending; restart delivers history to Saved", flush=True)
    request("/Auth/Logout", {}, expected=204)
    request("/Auth/Me", expected=401)


if __name__ == "__main__":
    main()
