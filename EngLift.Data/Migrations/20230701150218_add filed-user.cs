using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngLift.Data.Migrations
{
    public partial class addfileduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateTimeOffset",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: -420);

            migrationBuilder.AddColumn<int>(
                name: "TotalDateStudied",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalWords",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalWords",
                table: "UserLessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeOffset",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalDateStudied",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalWords",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalWords",
                table: "UserLessons");
        }
    }
}
