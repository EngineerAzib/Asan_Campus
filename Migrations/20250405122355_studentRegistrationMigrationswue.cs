using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asan_Campus.Migrations
{
    /// <inheritdoc />
    public partial class studentRegistrationMigrationswue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_SemesterRegistration_SemesterRegistrationid",
                table: "Prerequisites");

            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_SemesterRegistration_SemesterRegistrationid1",
                table: "Prerequisites");

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

            migrationBuilder.AlterColumn<int>(
                name: "studentRegistrated",
                table: "SemesterRegistration",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "startDate",
                table: "SemesterRegistration",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "endDate",
                table: "SemesterRegistration",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "studentRegistrated",
                table: "SemesterRegistration",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "startDate",
                table: "SemesterRegistration",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "endDate",
                table: "SemesterRegistration",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SemesterRegistrationid",
                table: "Prerequisites",
                column: "SemesterRegistrationid");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SemesterRegistrationid1",
                table: "Prerequisites",
                column: "SemesterRegistrationid1");

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
    }
}
