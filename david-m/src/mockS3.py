#!/usr/bin/env python3

import boto3
import os
import subprocess
import time
from moto import mock_aws

def start_moto_server():
    """Start the Moto S3 server in the background"""
    print("Starting Moto S3 server...")
    # Use subprocess to run the server in background
    process = subprocess.Popen(
        ["python", "-m", "moto.server", "-p", "5000"],
        stdout=subprocess.DEVNULL,
        stderr=subprocess.DEVNULL
    )
    # Wait a moment for server to start
    time.sleep(2)
    return process

# Function to create bucket and upload sample EDI file
def setup_edi_bucket():
    """Set up the EDI bucket with sample data"""
    print("Setting up EDI bucket...")
    try:
        # Create S3 client pointing to local moto server
        s3_client = boto3.client('s3',
                                 endpoint_url='http://localhost:5000',
                                 aws_access_key_id='fake',
                                 aws_secret_access_key='fake')

        # Create bucket
        bucket_name = 'edi-837-bucket'
        s3_client.create_bucket(Bucket=bucket_name)

        # Read sample EDI file
        sample_file_path = os.path.join('david-m', 'samples', 'ClaimPayment.txt')
        with open(sample_file_path, 'r') as f:
            edi_content = f.read()

        # Upload EDI file to bucket
        s3_client.put_object(Bucket=bucket_name,
                            Key='sample-837.edi',
                            Body=edi_content)

        print(f"✓ Created bucket '{bucket_name}'")
        print("✓ Uploaded sample EDI file 'sample-837.edi'")
        print("Mock S3 environment is ready!")

    except Exception as e:
        print(f"✗ Setup failed: {e}")

if __name__ == '__main__':
    print("Setting up Mock S3 Environment for EDI Processing...")
    print("=" * 50)

    # Start the server first
    server_process = start_moto_server()

    # Then set up the bucket
    setup_edi_bucket()

    print("\nServer is running on http://localhost:5000")
    print("Press Ctrl+C to stop the server")

    try:
        # Keep running - user can kill with Ctrl+C
        server_process.wait()
    except KeyboardInterrupt:
        print("\nShutting down Moto server...")
        server_process.terminate()
