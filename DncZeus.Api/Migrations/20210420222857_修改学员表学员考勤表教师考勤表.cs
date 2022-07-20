using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 修改学员表学员考勤表教师考勤表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CourseScheduleGuid",
                table: "TraineesAttence",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherAttenceGuid",
                table: "TraineesAttence",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TraineesGuid",
                table: "TraineesAttence",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TraineesAttence_TeacherAttenceGuid",
                table: "TraineesAttence",
                column: "TeacherAttenceGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TraineesAttence_TraineesGuid",
                table: "TraineesAttence",
                column: "TraineesGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_TraineesAttence_TeacherAttence_TeacherAttenceGuid",
                table: "TraineesAttence",
                column: "TeacherAttenceGuid",
                principalTable: "TeacherAttence",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TraineesAttence_Trainees_TraineesGuid",
                table: "TraineesAttence",
                column: "TraineesGuid",
                principalTable: "Trainees",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TraineesAttence_TeacherAttence_TeacherAttenceGuid",
                table: "TraineesAttence");

            migrationBuilder.DropForeignKey(
                name: "FK_TraineesAttence_Trainees_TraineesGuid",
                table: "TraineesAttence");

            migrationBuilder.DropIndex(
                name: "IX_TraineesAttence_TeacherAttenceGuid",
                table: "TraineesAttence");

            migrationBuilder.DropIndex(
                name: "IX_TraineesAttence_TraineesGuid",
                table: "TraineesAttence");

            migrationBuilder.DropColumn(
                name: "TeacherAttenceGuid",
                table: "TraineesAttence");

            migrationBuilder.DropColumn(
                name: "TraineesGuid",
                table: "TraineesAttence");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseScheduleGuid",
                table: "TraineesAttence",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
