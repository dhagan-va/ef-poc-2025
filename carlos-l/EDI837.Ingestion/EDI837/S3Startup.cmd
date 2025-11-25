
SET AWS_ACCESS_KEY_ID=testing&& SET AWS_SECRET_ACCESS_KEY=testing&& SET AWS_REGION=us-east-1&& aws s3 mb s3://edi-bucket --endpoint-url http://localhost:5001
SET AWS_ACCESS_KEY_ID=testing&& SET AWS_SECRET_ACCESS_KEY=testing&& SET AWS_REGION=us-east-1&& aws --endpoint-url=http://localhost:5001 s3 cp samples\837File.edi s3://edi-bucket/837File.edi
SET AWS_ACCESS_KEY_ID=testing&& SET AWS_SECRET_ACCESS_KEY=testing&& SET AWS_REGION=us-east-1&& aws --endpoint-url=http://localhost:5001 s3 cp samples\837File2.edi s3://edi-bucket/837File2.edi
SET AWS_ACCESS_KEY_ID=testing&& SET AWS_SECRET_ACCESS_KEY=testing&& SET AWS_REGION=us-east-1&& aws --endpoint-url=http://localhost:5001 s3 cp samples\275File.edi s3://edi-bucket/275File.edi



