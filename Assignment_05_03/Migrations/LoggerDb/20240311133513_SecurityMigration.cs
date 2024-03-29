using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_05_03.Migrations.LoggerDb
{
    /// <inheritdoc />
    public partial class SecurityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorLoggers",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestedController = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    RequestAction = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", maxLength: 9000, nullable: false),
                    RequestHeaders = table.Column<string>(type: "nvarchar(max)", maxLength: 9000, nullable: false),
                    RequestUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLoggers", x => x.RequestID);
                });

            migrationBuilder.CreateTable(
                name: "Loggers",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestedController = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    RequestAction = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", maxLength: 9000, nullable: false),
                    RequestHeaders = table.Column<string>(type: "nvarchar(max)", maxLength: 9000, nullable: true),
                    RequestUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggers", x => x.RequestID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLoggers");

            migrationBuilder.DropTable(
                name: "Loggers");
        }
    }
}
