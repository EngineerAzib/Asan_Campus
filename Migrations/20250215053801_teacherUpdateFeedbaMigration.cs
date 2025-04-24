using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class teacherUpdateFeedbaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "department",
                table: "Teachers");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentID",
                table: "Teachers",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DepartmentID",
                table: "Teachers",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DepartmentID",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DepartmentID",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
