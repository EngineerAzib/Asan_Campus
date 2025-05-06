using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class notiuodatemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Notifications",
                newName: "date");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SemesterID",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DepartmentID",
                table: "Notifications",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SemesterID",
                table: "Notifications",
                column: "SemesterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Departments_DepartmentID",
                table: "Notifications",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Semesters_SemesterID",
                table: "Notifications",
                column: "SemesterID",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Departments_DepartmentID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Semesters_SemesterID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DepartmentID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SemesterID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SemesterID",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Notifications",
                newName: "Priority");
        }
    }
}
