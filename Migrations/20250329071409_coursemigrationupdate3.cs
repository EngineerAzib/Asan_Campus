using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class coursemigrationupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseSchedules_CourseScheduleId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseScheduleId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseScheduleId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "courseId",
                table: "CourseSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_courseId",
                table: "CourseSchedules",
                column: "courseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedules_Courses_courseId",
                table: "CourseSchedules",
                column: "courseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedules_Courses_courseId",
                table: "CourseSchedules");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedules_courseId",
                table: "CourseSchedules");

            migrationBuilder.DropColumn(
                name: "courseId",
                table: "CourseSchedules");

            migrationBuilder.AddColumn<int>(
                name: "CourseScheduleId",
                table: "Courses",
                type: "int",
                nullable: true);

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
    }
}
