#!/bin/sh
docker run --env-file ../.env -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:latest
