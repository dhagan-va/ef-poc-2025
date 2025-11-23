aws s3 mb s3://edi-bucket --endpoint-url http://localhost:5001
aws --endpoint-url=http://localhost:5001 s3 cp C:\Users\CarlosLadino\source\repos\ef-poc-2025\carlos-l\EDI837.Ingestion\EDI837\samples\837File.edi s3://edi-bucket/837File.edi



