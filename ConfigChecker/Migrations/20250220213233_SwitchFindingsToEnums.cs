using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigChecker.Migrations
{
    /// <inheritdoc />
    public partial class SwitchFindingsToEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Findings",
                type: "INTEGER",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Findings",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 200);
        }
    }
}
