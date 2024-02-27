using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JournalAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogLevel",
                table: "LogMessages");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "LogMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "LogMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "LogMessages");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "LogMessages");

            migrationBuilder.AddColumn<int>(
                name: "LogLevel",
                table: "LogMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
