IF DB_ID(N'PayerEdi') IS NULL
BEGIN
    CREATE DATABASE [PayerEdi];
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = N'payeredi_app')
BEGIN
    CREATE LOGIN [payeredi_app]
    WITH PASSWORD = '$(PAYEREDI_APP_PASSWORD)',
        CHECK_POLICY = OFF,
        CHECK_EXPIRATION = OFF;
END;
GO

USE [PayerEdi];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'payeredi_app')
BEGIN
    CREATE USER [payeredi_app] FOR LOGIN [payeredi_app];
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.database_role_members role_members
    INNER JOIN sys.database_principals roles ON roles.principal_id = role_members.role_principal_id
    INNER JOIN sys.database_principals members ON members.principal_id = role_members.member_principal_id
    WHERE roles.name = N'db_datareader' AND members.name = N'payeredi_app'
)
BEGIN
    ALTER ROLE [db_datareader] ADD MEMBER [payeredi_app];
END;

IF NOT EXISTS
(
    SELECT 1
    FROM sys.database_role_members role_members
    INNER JOIN sys.database_principals roles ON roles.principal_id = role_members.role_principal_id
    INNER JOIN sys.database_principals members ON members.principal_id = role_members.member_principal_id
    WHERE roles.name = N'db_datawriter' AND members.name = N'payeredi_app'
)
BEGIN
    ALTER ROLE [db_datawriter] ADD MEMBER [payeredi_app];
END;
GO
