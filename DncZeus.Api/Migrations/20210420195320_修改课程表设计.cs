using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 修改课程表设计 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "EndMonth",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "StartDay",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                table: "CourseSchedule");

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LoopOfYear",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "CourseHour",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "LoopOfYear",
                table: "CourseSchedule");

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                table: "CourseSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndMonth",
                table: "CourseSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartDay",
                table: "CourseSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                table: "CourseSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "CourseHour",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
