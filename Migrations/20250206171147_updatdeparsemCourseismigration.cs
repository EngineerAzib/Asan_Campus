using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class updatdeparsemCourseismigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_departmentSemesters",
                table: "departmentSemesters");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "departmentSemesters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_departmentSemesters",
                table: "departmentSemesters",
                columns: new[] { "DepartmentId", "SemesterId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_departmentSemesters_CourseId",
                table: "departmentSemesters",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_departmentSemesters_Courses_CourseId",
                table: "departmentSemesters",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                 onDelete: ReferentialAction.Restrict);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departmentSemesters_Courses_CourseId",
                table: "departmentSemesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departmentSemesters",
                table: "departmentSemesters");

            migrationBuilder.DropIndex(
                name: "IX_departmentSemesters_CourseId",
                table: "departmentSemesters");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "departmentSemesters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departmentSemesters",
                table: "departmentSemesters",
                columns: new[] { "DepartmentId", "SemesterId" });
        }
    }
}
