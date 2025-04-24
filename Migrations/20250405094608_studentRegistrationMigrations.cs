using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class studentRegistrationMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SemesterRegistrationid",
                table: "Prerequisites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SemesterRegistrationid1",
                table: "Prerequisites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SemesterRegistration",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentRegistrated = table.Column<int>(type: "int", nullable: false),
                    MaxStudent = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    SemesterID = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterRegistration", x => x.id);
                    table.ForeignKey(
                        name: "FK_SemesterRegistration_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SemesterRegistration_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterRegistration_Semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SemesterRegistrationid",
                table: "Prerequisites",
                column: "SemesterRegistrationid");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SemesterRegistrationid1",
                table: "Prerequisites",
                column: "SemesterRegistrationid1");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterRegistration_CourseId",
                table: "SemesterRegistration",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterRegistration_DepartmentID",
                table: "SemesterRegistration",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterRegistration_SemesterID",
                table: "SemesterRegistration",
                column: "SemesterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequisites_SemesterRegistration_SemesterRegistrationid",
                table: "Prerequisites",
                column: "SemesterRegistrationid",
                principalTable: "SemesterRegistration",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequisites_SemesterRegistration_SemesterRegistrationid1",
                table: "Prerequisites",
                column: "SemesterRegistrationid1",
                principalTable: "SemesterRegistration",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_SemesterRegistration_SemesterRegistrationid",
                table: "Prerequisites");

            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_SemesterRegistration_SemesterRegistrationid1",
                table: "Prerequisites");

            migrationBuilder.DropTable(
                name: "SemesterRegistration");

            migrationBuilder.DropIndex(
                name: "IX_Prerequisites_SemesterRegistrationid",
                table: "Prerequisites");

            migrationBuilder.DropIndex(
                name: "IX_Prerequisites_SemesterRegistrationid1",
                table: "Prerequisites");

            migrationBuilder.DropColumn(
                name: "SemesterRegistrationid",
                table: "Prerequisites");

            migrationBuilder.DropColumn(
                name: "SemesterRegistrationid1",
                table: "Prerequisites");
        }
    }
}
