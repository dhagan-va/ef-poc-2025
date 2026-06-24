#!/usr/bin/env python3

import boto3
import os
from moto import mock_aws

# Function to create bucket and upload sample EDI file
@mock_aws
def setup_edi_bucket():
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

    print(f"Created bucket '{bucket_name}' and uploaded sample EDI file.")

if __name__ == '__main__':
    setup_edi_bucket()
