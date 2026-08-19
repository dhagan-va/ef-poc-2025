Moto-based S3 integration helper
================================

Prerequisites
-------------
- Python 3.8+
- pip
- A virtual environment (recommended)

Install required Python packages (inside the venv):

  python -m pip install --upgrade pip
  python -m pip install "moto[server]" boto3 requests

Overview
--------
This helper uses moto (a local mock of AWS services) to provide an S3-compatible endpoint for
testing the Deepika.EDIIngestion application without contacting real AWS. The helper will start
a moto S3 server, create a test bucket, upload sample EDI files from the samples/ folder, and
then run the .NET app configured to read from the mock S3.

What it does
------------
- Starts moto_server (S3) on port 5000
- Creates bucket `test-edi-bucket` and uploads sample EDI files from `deepika-macherla_d/samples`
- Sets `AWS_S3_ENDPOINT` and `S3_BUCKET` environment variables and runs the .NET app so it processes the mock S3 objects

Quick local steps (PowerShell)
-----------------------------
Run these from the repository root: C:\Deepika\SAIC\Project

1) Activate the virtual environment (one-time per shell)
. .\.venv\Scripts\Activate.ps1

2) Start moto S3 server (Terminal A)
python -u -m moto.server -p 5000

3) Upload sample files to the mock bucket (Terminal B)
. .\.venv\Scripts\python.exe -c "import boto3, pathlib; s3=boto3.client('s3',endpoint_url='http://127.0.0.1:5000',aws_access_key_id='test',aws_secret_access_key='test'); s3.create_bucket(Bucket='test-edi-bucket'); [s3.put_object(Bucket='test-edi-bucket', Key='incoming/'+p.name, Body=p.read_bytes()) for p in pathlib.Path('deepika-macherla_d/samples').glob('sample_*.edi')]; print('uploaded incoming')"

4) Verify uploaded objects
. .\.venv\Scripts\python.exe -c "import boto3; s3=boto3.client('s3',endpoint_url='http://127.0.0.1:5000',aws_access_key_id='test',aws_secret_access_key='test'); print([o['Key'] for o in s3.list_objects_v2(Bucket='test-edi-bucket').get('Contents',[])])"

5) Run the .NET app against the mock S3 (same terminal B)
$env:AWS_S3_ENDPOINT="http://127.0.0.1:5000"; $env:S3_BUCKET="test-edi-bucket"; dotnet run --project .\deepika-macherla_d\src\Deepika.EDIIngestion

Where logs appear
-----------------
- The app writes logs to the app output folder when run (example):
  deepika-macherla_d\src\Deepika.EDIIngestion\bin\Debug\net8.0\logs\edi-ingestion-*.log
- To tail logs while running (PowerShell):
  Get-Content '<path-to-log-file>' -Wait

Notes / troubleshooting
-----------------------
- If moto_server requires extra dependencies: python -m pip install "moto[server]" boto3 requests
- Use AWS_ACCESS_KEY_ID/AWS_SECRET_ACCESS_KEY = "test" for local moto; the S3EdiStorageService is configured to use the ServiceURL and path-style addressing for moto.
- For Visual Studio debug runs we set environmentVariables in launchSettings.json so F5 uses the mock S3. Restart VS after changing user-level env vars (setx).

