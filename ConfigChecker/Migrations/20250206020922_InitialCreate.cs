using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigChecker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Findings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReportId = table.Column<string>(type: "TEXT", nullable: false),
                    ResourceName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false),
                    CweId = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Findings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Findings_ReportId",
                table: "Findings",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Findings");
        }
    }
}
