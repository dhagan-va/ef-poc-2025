import boto3, os
from pathlib import Path

s3 = boto3.client("s3", endpoint_url = "http://127.0.0.1:4566", aws_access_key_id="fake-access-key",aws_secret_access_key="fake-secret-key")
bucket = "test-bucket"
s3.create_bucket(Bucket=bucket)

current_dir = Path(__file__).resolve().parent
samples_folder = current_dir.parent.parent / "samples"
for filename in os.listdir(samples_folder):
    if not filename.lower().startswith("test") and filename.lower().endswith(".edi"):
        path = os.path.join(samples_folder, filename)
        with open(path, "rb") as f:
            s3.put_object(Bucket=bucket,Key=filename,Body=f)
            print("Uploaded")

print("--------------------------")
print("---Verify uploaded file---")
print(s3.list_objects_v2(Bucket = bucket))