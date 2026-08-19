"""
Simple moto-based integration script (standalone, not pytest) that:
- Starts a moto_server S3 process (requires moto to be installed and available on PATH)
- Creates a bucket and uploads sample files from samples/
- Sets AWS_S3_ENDPOINT to point to moto_server and runs the .NET app in S3 mode

Usage:
  pip install moto
  python tests/moto_s3_integration.py

Notes:
- moto_server must be available on PATH. If not, run `python -m moto.server s3 -p 5000` instead.
- This script is a convenience helper for local testing; CI should run moto via a reproducible step.
"""

import os
import subprocess
import time
import requests
import sys
from pathlib import Path

MOTO_PORT = 5000
MOTO_URL = f"http://127.0.0.1:{MOTO_PORT}"
BUCKET = "test-edi-bucket"
SCRIPT_DIR = Path(__file__).parent.parent
SAMPLES_DIR = SCRIPT_DIR / "samples"


def start_moto_server():
	# Try to start moto_server
	print("Starting moto_server on port", MOTO_PORT)
	proc = subprocess.Popen([sys.executable, "-m", "moto.server", "s3", "-p", str(MOTO_PORT)], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
	# wait for server to be up
	for i in range(10):
		try:
			r = requests.get(MOTO_URL)
			break
		except Exception:
			time.sleep(0.5)
	else:
		raise RuntimeError("moto_server did not start")
	return proc


def upload_samples():
	import boto3
	s3 = boto3.client('s3', endpoint_url=MOTO_URL, aws_access_key_id='test', aws_secret_access_key='test')
	s3.create_bucket(Bucket=BUCKET)
	for p in SAMPLES_DIR.glob('sample_*.edi'):
		print("Uploading", p.name)
		s3.put_object(Bucket=BUCKET, Key=p.name, Body=p.read_bytes())


def run_dotnet_app():
	env = os.environ.copy()
	env['AWS_S3_ENDPOINT'] = MOTO_URL
	env['S3_BUCKET'] = BUCKET
	# Ensure dotnet run uses the same working directory as the project
	proj = SCRIPT_DIR / "src" / "Deepika.EDIIngestion"
	print("Running dotnet app against moto S3...")
	subprocess.run(["dotnet", "run", "--project", str(proj)], env=env, check=True)


if __name__ == '__main__':
	proc = start_moto_server()
	try:
		upload_samples()
		run_dotnet_app()
	finally:
		proc.terminate()
		proc.wait()
