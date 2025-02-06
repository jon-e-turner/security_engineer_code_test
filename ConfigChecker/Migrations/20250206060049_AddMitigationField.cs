using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigChecker.Migrations
{
    /// <inheritdoc />
    public partial class AddMitigationField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mitigation",
                table: "Findings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mitigation",
                table: "Findings");
        }
    }
}
