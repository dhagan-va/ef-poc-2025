import boto3, os
from pathlib import Path

s3 = boto3.client("s3", endpoint_url = "http://127.0.0.1:4566", aws_access_key_id="fake-access-key",aws_secret_access_key="fake-secret-key")
bucket = "test-bucket"
print("--------------------------")
print("---Listing uploaded file---")
print(s3.list_objects_v2(Bucket = bucket))