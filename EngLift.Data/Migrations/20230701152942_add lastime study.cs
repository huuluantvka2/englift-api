using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngLift.Data.Migrations
{
    public partial class addlastimestudy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeStudy",
                table: "Users",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeStudy",
                table: "Users");
        }
    }
}
