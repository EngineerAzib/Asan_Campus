using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class updatInternshipmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Internships");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stippend",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "departmentId",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Internships_departmentId",
                table: "Internships",
                column: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Internships_Departments_departmentId",
                table: "Internships",
                column: "departmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Internships_Departments_departmentId",
                table: "Internships");

            migrationBuilder.DropIndex(
                name: "IX_Internships_departmentId",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "Stippend",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "departmentId",
                table: "Internships");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Internships",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
