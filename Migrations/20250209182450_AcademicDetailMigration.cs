using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class AcademicDetailMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcadmicDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gpa = table.Column<double>(type: "float", nullable: false),
                    grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentId = table.Column<int>(type: "int", nullable: false),
                    courseId = table.Column<int>(type: "int", nullable: false),
                    semesterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcadmicDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_AcadmicDetails_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcadmicDetails_Semesters_semesterId",
                        column: x => x.semesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);  // Changed here to Restrict
                    table.ForeignKey(
                        name: "FK_AcadmicDetails_Students_studentId",
                        column: x => x.studentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict); ;
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcadmicDetails_courseId",
                table: "AcadmicDetails",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_AcadmicDetails_semesterId",
                table: "AcadmicDetails",
                column: "semesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AcadmicDetails_studentId",
                table: "AcadmicDetails",
                column: "studentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcadmicDetails");
        }
    }
}
