#!/bin/sh
docker exec -it sql1 bash -c \
  '/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -d HIPAA -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES" -No'