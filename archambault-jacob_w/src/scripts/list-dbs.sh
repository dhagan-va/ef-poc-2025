#!/bin/sh
docker exec -it sql1 bash -c \
  '/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -Q "SELECT name FROM sys.databases;" -No'