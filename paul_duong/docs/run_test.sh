#!/bin/bash

cd ../tests/EDI837IngestionTaskTests

dotnet test --settings ../../coverlet.runsettings --collect:"XPlat Code Coverage"
reportgenerator -reports:"TestResults/**/*.xml" -targetdir:"TestResults/coverage-report" -reporttypes:Html -assemblyfilters:"-xunit.*;-Microsoft.*;-System.*;-EDI837IngestionTask.Migrations.*"  -classfilters:"-EDI837IngestionTask.Migrations.*;-EDI837IngestionTask.HIPAA_5010_837P.*ContextModelSnapshot"
start TestResults/coverage-report/index.html