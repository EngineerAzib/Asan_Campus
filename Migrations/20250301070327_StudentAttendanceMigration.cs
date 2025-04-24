using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class StudentAttendanceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentAttendances",
                columns: table => new
                {
                    StudentAttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: true), // Nullable
                    CourseID = table.Column<int>(type: "int", nullable: true), // Nullable
                    SemesterID = table.Column<int>(type: "int", nullable: true), // Nullable
                    TotalClasses = table.Column<int>(type: "int", nullable: false),
                    AttendedClasses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendances", x => x.StudentAttendanceID);
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // Changed to Restrict
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // Changed to Restrict
                    table.ForeignKey(
                        name: "FK_StudentAttendances_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); // Changed to Restrict
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_CourseID",
                table: "StudentAttendances",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_SemesterID",
                table: "StudentAttendances",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentID",
                table: "StudentAttendances",
                column: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAttendances");
        }
    }
}
