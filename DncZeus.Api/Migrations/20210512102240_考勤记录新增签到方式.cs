using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 考勤记录新增签到方式 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttenceType",
                table: "TraineesAttence",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttenceType",
                table: "TeacherAttence",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttenceType",
                table: "TraineesAttence");

            migrationBuilder.DropColumn(
                name: "AttenceType",
                table: "TeacherAttence");
        }
    }
}
