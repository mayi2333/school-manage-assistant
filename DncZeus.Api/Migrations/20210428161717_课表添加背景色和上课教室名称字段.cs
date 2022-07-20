using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 课表添加背景色和上课教室名称字段 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackColor",
                table: "CourseSchedule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassRoomName",
                table: "CourseSchedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackColor",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "ClassRoomName",
                table: "CourseSchedule");
        }
    }
}
