using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class updatdeparsemismigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentSemester_Departments_DepartmentId",
                table: "DepartmentSemester");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentSemester_Semesters_SemesterId",
                table: "DepartmentSemester");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentSemester",
                table: "DepartmentSemester");

            migrationBuilder.RenameTable(
                name: "DepartmentSemester",
                newName: "departmentSemesters");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentSemester_SemesterId",
                table: "departmentSemesters",
                newName: "IX_departmentSemesters_SemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departmentSemesters",
                table: "departmentSemesters",
                columns: new[] { "DepartmentId", "SemesterId" });

            migrationBuilder.AddForeignKey(
                name: "FK_departmentSemesters_Departments_DepartmentId",
                table: "departmentSemesters",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_departmentSemesters_Semesters_SemesterId",
                table: "departmentSemesters",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departmentSemesters_Departments_DepartmentId",
                table: "departmentSemesters");

            migrationBuilder.DropForeignKey(
                name: "FK_departmentSemesters_Semesters_SemesterId",
                table: "departmentSemesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departmentSemesters",
                table: "departmentSemesters");

            migrationBuilder.RenameTable(
                name: "departmentSemesters",
                newName: "DepartmentSemester");

            migrationBuilder.RenameIndex(
                name: "IX_departmentSemesters_SemesterId",
                table: "DepartmentSemester",
                newName: "IX_DepartmentSemester_SemesterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentSemester",
                table: "DepartmentSemester",
                columns: new[] { "DepartmentId", "SemesterId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentSemester_Departments_DepartmentId",
                table: "DepartmentSemester",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentSemester_Semesters_SemesterId",
                table: "DepartmentSemester",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
