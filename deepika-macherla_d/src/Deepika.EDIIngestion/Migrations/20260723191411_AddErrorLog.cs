using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Deepika.EDIIngestion.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No identity column changes required for existing tables.

            // Create ErrorLogs table only if it does not already exist to handle databases created by EnsureCreated()
            migrationBuilder.Sql(@"IF OBJECT_ID(N'dbo.ErrorLogs', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ErrorLogs](
        [Id] int NOT NULL IDENTITY(1,1),
        [FileName] nvarchar(260) NULL,
        [OccurredAt] datetime2 NOT NULL,
        [ErrorMessage] nvarchar(2000) NULL,
        [ErrorType] nvarchar(250) NULL,
        [StackTrace] nvarchar(max) NULL,
        [FileSnippet] nvarchar(max) NULL,
        CONSTRAINT [PK_ErrorLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop ErrorLogs only if it exists
            migrationBuilder.Sql(@"IF OBJECT_ID(N'dbo.ErrorLogs', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[ErrorLogs];
END");

            // No identity column changes to revert.
        }
    }
}
