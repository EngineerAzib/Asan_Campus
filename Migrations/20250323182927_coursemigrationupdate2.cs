using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class coursemigrationupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "CourseScheduleId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    room = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSchedules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseScheduleId",
                table: "Courses",
                column: "CourseScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseSchedules_CourseScheduleId",
                table: "Courses",
                column: "CourseScheduleId",
                principalTable: "CourseSchedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseSchedules_CourseScheduleId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "CourseSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseScheduleId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseScheduleId",
                table: "Courses");

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
    }
}
