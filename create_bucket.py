import boto3
import os

def create_bucket(bucket_name):
    s3 = boto3.client(
        "s3",
        endpoint_url="http://127.0.0.1:5000",
        aws_access_key_id="test",
        aws_secret_access_key="test",
        region_name="us-east-1"
    )

    s3.create_bucket(Bucket=bucket_name)
    print(f"Bucket created: {bucket_name}")

    # Read sample EDI file
    sample_file_path = os.path.join('david-m', 'samples', 'ClaimPayment.txt')
    with open(sample_file_path, 'r') as f:
        edi_content = f.read()

    # Upload EDI file to bucket
    s3.put_object(Bucket=bucket_name,
                        Key='sample-837.edi',
                        Body=edi_content)

    print(f"Uploaded sample EDI file.")

if __name__ == "__main__":
    create_bucket("edi-837-bucket")
