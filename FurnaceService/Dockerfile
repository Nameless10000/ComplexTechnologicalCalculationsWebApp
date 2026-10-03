FROM python:3.12-slim

WORKDIR /app

COPY FurnaceService/requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt

COPY FurnaceService/app ./app
COPY FurnaceService/generated ./generated
COPY FurnaceService/generate_proto.py ./generate_proto.py
COPY Contracts/Protos /Contracts/Protos
RUN python generate_proto.py

EXPOSE 8080

CMD ["python", "-m", "app.server"]
