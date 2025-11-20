import boto3
import os

# --- Configuration ---
MOTO_ENDPOINT = "http://localhost:7808"
BUCKET_NAME = "edi-test-bucket"
LOCAL_FILE_PATH = "C:\\Users\\fsr-admin\\source\\repos\\ef-poc-2025\\johnny_booth\\samples\\837-sample-file.edi" 
S3_OBJECT_KEY = "837-sample-file.edi"

# --- Credentials ---
# Moto requires *some* credentials, even if they are fake.
DUMMY_CREDENTIALS = {
    "aws_access_key_id": "local_access_key",
    "aws_secret_access_key": "local_secret_key",
    "region_name": "us-east-1",
    "endpoint_url": MOTO_ENDPOINT,
}

if not os.path.exists(LOCAL_FILE_PATH):
    print(f"Error: Local file not found at {LOCAL_FILE_PATH}")
    exit(1)

try:
    s3_client = boto3.client("s3", **DUMMY_CREDENTIALS)
    
    # 1. Create the bucket (it must exist first)
    print(f"Creating bucket: {BUCKET_NAME}")
    s3_client.create_bucket(Bucket=BUCKET_NAME)

    # 2. Upload your local file to the mock S3 server
    print(f"Uploading {os.path.basename(LOCAL_FILE_PATH)} to s3://{BUCKET_NAME}/{S3_OBJECT_KEY}")
    s3_client.upload_file(
        Filename=LOCAL_FILE_PATH,
        Bucket=BUCKET_NAME,
        Key=S3_OBJECT_KEY
    )
    
    print("\n✅ Setup complete! File is ready in the mock S3 bucket.")
    
except Exception as e:
    print(f"\n❌ An error occurred during setup: {e}")