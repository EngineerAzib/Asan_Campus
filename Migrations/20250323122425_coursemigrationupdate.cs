using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class coursemigrationupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "days",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "endDate",
                table: "Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "Courses",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "days",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "endDate",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "Courses");
        }
    }
}
