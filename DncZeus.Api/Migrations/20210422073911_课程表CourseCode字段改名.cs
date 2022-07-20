using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 课程表CourseCode字段改名 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_CourseSubject_CourseSubjectCode",
                table: "CourseSchedule");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedule_CourseSubjectCode",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "CourseSubjectCode",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "CourseSubjectGuid",
                table: "CourseSchedule");

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_CourseCode",
                table: "CourseSchedule",
                column: "CourseCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_CourseSubject_CourseCode",
                table: "CourseSchedule",
                column: "CourseCode",
                principalTable: "CourseSubject",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_CourseSubject_CourseCode",
                table: "CourseSchedule");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedule_CourseCode",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "CourseSchedule");

            migrationBuilder.AddColumn<string>(
                name: "CourseSubjectCode",
                table: "CourseSchedule",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseSubjectGuid",
                table: "CourseSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_CourseSubjectCode",
                table: "CourseSchedule",
                column: "CourseSubjectCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_CourseSubject_CourseSubjectCode",
                table: "CourseSchedule",
                column: "CourseSubjectCode",
                principalTable: "CourseSubject",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
