#!/bin/bash

cd ../src/EDI837IngestionTask

dotnet ef database drop --context HIPAA_5010_837P_Context -f
dotnet ef database update --context HIPAA_5010_837P_Context