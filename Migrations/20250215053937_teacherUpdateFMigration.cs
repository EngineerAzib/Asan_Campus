using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class teacherUpdateFMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DepartmentID",
                table: "Teachers");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Teachers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DepartmentID",
                table: "Teachers",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DepartmentID",
                table: "Teachers");

            migrationBuilder.AlterColumn<int?>(
      name: "DepartmentID",
      table: "Teachers",
      type: "int",
      nullable: true,
      oldClrType: typeof(int),
      oldType: "int",
      oldNullable: false);


            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DepartmentID",
                table: "Teachers",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
