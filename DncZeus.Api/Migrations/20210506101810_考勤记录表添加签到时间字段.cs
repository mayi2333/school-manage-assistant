using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 考勤记录表添加签到时间字段 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AttenceTime",
                table: "TraineesAttence",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AttenceTime",
                table: "TeacherAttence",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttenceTime",
                table: "TraineesAttence");

            migrationBuilder.DropColumn(
                name: "AttenceTime",
                table: "TeacherAttence");
        }
    }
}
