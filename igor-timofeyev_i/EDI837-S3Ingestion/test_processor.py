import os
import sys

os.environ["AWS_ACCESS_KEY_ID"] = "testing"
os.environ["AWS_SECRET_ACCESS_KEY"] = "testing"
os.environ["AWS_SECURITY_TOKEN"] = "testing"
os.environ["AWS_SESSION_TOKEN"] = "testing"
os.environ["AWS_DEFAULT_REGION"] = "us-east-1"

import asyncio
import subprocess
import boto3
import pytest
from moto import mock_aws
from dotenv import load_dotenv


SAMPLE_837_EDI = (
    "ISA*00*          *00*          *ZZ*SUBMITTER99    *ZZ*RECEIVER88     *260708*1500*^*00501*000000001*1*T*:~\n"
    "GS*HC*SUBMITTER99*RECEIVER88*20260708*1500*1*X*005010X222A1~\n"
    "ST*837*0021*005010X222A1~\n"
    "BHT*0019*00*244579*20061015*1023*CH~\n"
    "NM1*41*2*PREMIER BILLING SERVICE*****46*TGJ23~\n"
    "PER*IC*JERRY*TE*3055552222*EX*231~\n"
    "NM1*40*2*KEY INSURANCE COMPANY*****46*66783JJT~\n"
    "HL*1**20*1~\n"
    "PRV*BI*PXC*203BF0100Y~\n"
    "NM1*85*2*BEN KILDARE SERVICE*****XX*9876543210~\n"
    "N3*234 SEAWAY ST~\n"
    "N4*MIAMI*FL*33111~\n"
    "REF*EI*587654321~\n"
    "NM1*87*2*Kildare Associates~\n"
    "N3*2345 OCEAN BLVD~\n"
    "N4*MIAMI*FL*33111~\n"
    "HL*2*1*22*1~\n"
    "SBR*P**2222-SJ******CI~\n"
    "NM1*IL*1*SMITH*JANE****MI*JS00111223333~\n"
    "N3*123 SUBSCRIBER ROAD~\n"
    "N4*MIAMI*FL*33112~\n"
    "DMG*D8*19430501*F~\n"
    "NM1*PR*2*KEY INSURANCE COMPANY*****PI*999996666~\n"
    "REF*G2*KA6663~\n"
    "HL*3*2*23*0~\n"
    "PAT*19~\n"
    "NM1*QC*1*SMITH*TED~\n"
    "N3*236 N MAIN ST~\n"
    "N4*MIAMI*FL*33413~\n"
    "DMG*D8*19730501*M~\n"
    "CLM*26463774*100***11:B:1*Y*A*Y*I~\n"
    "REF*D9*17312345600006351~\n"
    "HI*BK:0340*BF:V7389~\n"
    "LX*1~\n"
    "SV1*HC:99213*40*UN*1***1~\n"
    "DTP*472*D8*20061003~\n"
    "LX*2~\n"
    "SV1*HC:87070*15*UN*1***1~\n"
    "DTP*472*D8*20061003~\n"
    "LX*3~\n"
    "SV1*HC:99214*35*UN*1***2~\n"
    "DTP*472*D8*20061010~\n"
    "LX*4~\n"
    "SV1*HC:86663*10*UN*1***2~\n"
    "DTP*472*D8*20061010~\n"
    "SE*44*0021~\n"
    "GE*1*101~\n"
    "IEA*1*000000101~"
)

async def run_csharp_ingestion(edi_payload: str) -> tuple[int, str, str]:
    """Helper function to execute your compiled C# executable asynchronously."""

    load_dotenv()

    # Verified Path to your compiled C# app artifact
    #csharp_exe_path = r"C:\Projects\VA\EDI 837\igor-timofeyev_i\src\EDI837Ingestion\bin\Debug\net8.0\EDI837Ingestion.exe"
    csharp_exe_path = os.getenv("CSHARP_EXE_PATH")
    
    process = await asyncio.create_subprocess_exec(
        csharp_exe_path,
        stdin=subprocess.PIPE,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE
    )
    
    stdout, stderr = await process.communicate(input=edi_payload.encode('utf-8'))
    return process.returncode, stdout.decode().strip(), stderr.decode().strip()

@pytest.mark.asyncio
@mock_aws
async def test_edi_simulation_to_csharp():
    bucket_name = "mock-s3-ingestion-bucket"
    file_key = "claims/incoming_sample.edi"
    
    # 1. Arrange: Initialize Moto Mock S3 environment & push file
    s3_client = boto3.client("s3", region_name="us-east-1")
    s3_client.create_bucket(Bucket=bucket_name)
    s3_client.put_object(Bucket=bucket_name, Key=file_key, Body=SAMPLE_837_EDI)
    
    # 2. Act: Read the file back out asynchronously from the simulated bucket
    s3_object = s3_client.get_object(Bucket=bucket_name, Key=file_key)
    downloaded_edi = s3_object["Body"].read().decode("utf-8")
    
    # Pass the downloaded mock data straight into the compiled C# application
    return_code, stdout, stderr = await run_csharp_ingestion(downloaded_edi)
    
    # 3. Assert: Verify C# application processes the text and closes gracefully
    assert return_code == 0, f"C# app failed with error: {stderr}"
    assert "Success" in stdout or "ingested" in stdout.lower()

if __name__ == "__main__":
    # Force Python to create an async loop and run your test manually
    with mock_aws():
        asyncio.run(test_edi_simulation_to_csharp())