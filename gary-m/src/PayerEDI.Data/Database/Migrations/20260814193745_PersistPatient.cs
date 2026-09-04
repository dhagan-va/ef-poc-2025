using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayerEDI.Data.Database.Migrations
{
    /// <inheritdoc />
    public partial class PersistPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(
                        type: "nvarchar(10)",
                        maxLength: 10,
                        nullable: false
                    ),
                    EntityIdentifierCode = table.Column<string>(
                        type: "nvarchar(3)",
                        maxLength: 3,
                        nullable: true
                    ),
                    IdentificationCodeQualifier = table.Column<string>(
                        type: "nvarchar(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                    ResponseContactIdentifier = table.Column<string>(
                        type: "nvarchar(80)",
                        maxLength: 80,
                        nullable: true
                    ),
                    LastName = table.Column<string>(
                        type: "nvarchar(60)",
                        maxLength: 60,
                        nullable: true
                    ),
                    SecondLastName = table.Column<string>(
                        type: "nvarchar(60)",
                        maxLength: 60,
                        nullable: true
                    ),
                    FirstName = table.Column<string>(
                        type: "nvarchar(35)",
                        maxLength: 35,
                        nullable: true
                    ),
                    MiddleName = table.Column<string>(
                        type: "nvarchar(25)",
                        maxLength: 25,
                        nullable: true
                    ),
                    Prefix = table.Column<string>(
                        type: "nvarchar(10)",
                        maxLength: 10,
                        nullable: true
                    ),
                    Suffix = table.Column<string>(
                        type: "nvarchar(10)",
                        maxLength: 10,
                        nullable: true
                    ),
                    OrganizationName = table.Column<string>(
                        type: "nvarchar(60)",
                        maxLength: 60,
                        nullable: true
                    ),
                    AdditionalOrganizationName = table.Column<string>(
                        type: "nvarchar(60)",
                        maxLength: 60,
                        nullable: true
                    ),
                    Relationship = table.Column<string>(
                        type: "nvarchar(2)",
                        maxLength: 2,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Patients");
        }
    }
}
