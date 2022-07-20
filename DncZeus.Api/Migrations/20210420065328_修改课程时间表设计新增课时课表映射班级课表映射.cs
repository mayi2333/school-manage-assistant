using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 修改课程时间表设计新增课时课表映射班级课表映射 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseHour_Trainees_TraineesGuid",
                table: "CourseHour");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_ClassGrade_ClassGradeGuid",
                table: "CourseSchedule");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedule_ClassGradeGuid",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "ClassGradeGuid",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "CourseSchedule");

            migrationBuilder.AddColumn<string>(
                name: "CourseSubjectCode",
                table: "CourseSchedule",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseSubjectGuid",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndMonth",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsEnabled",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StartDay",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                table: "CourseSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsSpecial",
                table: "ClassGrade",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassGradeCourseScheduleMapping",
                columns: table => new
                {
                    ClassGradeGuid = table.Column<Guid>(nullable: false),
                    CourseScheduleGuid = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassGradeCourseScheduleMapping", x => new { x.ClassGradeGuid, x.CourseScheduleGuid });
                    table.ForeignKey(
                        name: "FK_ClassGradeCourseScheduleMapping_ClassGrade_ClassGradeGuid",
                        column: x => x.ClassGradeGuid,
                        principalTable: "ClassGrade",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassGradeCourseScheduleMapping_CourseSchedule_CourseScheduleGuid",
                        column: x => x.CourseScheduleGuid,
                        principalTable: "CourseSchedule",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseHourCourseScheduleMapping",
                columns: table => new
                {
                    CourseHourGuid = table.Column<Guid>(nullable: false),
                    CourseScheduleGuid = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseHourCourseScheduleMapping", x => new { x.CourseHourGuid, x.CourseScheduleGuid });
                    table.ForeignKey(
                        name: "FK_CourseHourCourseScheduleMapping_CourseHour_CourseHourGuid",
                        column: x => x.CourseHourGuid,
                        principalTable: "CourseHour",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseHourCourseScheduleMapping_CourseSchedule_CourseScheduleGuid",
                        column: x => x.CourseScheduleGuid,
                        principalTable: "CourseSchedule",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_CourseSubjectCode",
                table: "CourseSchedule",
                column: "CourseSubjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGradeCourseScheduleMapping_CourseScheduleGuid",
                table: "ClassGradeCourseScheduleMapping",
                column: "CourseScheduleGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CourseHourCourseScheduleMapping_CourseScheduleGuid",
                table: "CourseHourCourseScheduleMapping",
                column: "CourseScheduleGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseHour_Trainees_TraineesGuid",
                table: "CourseHour",
                column: "TraineesGuid",
                principalTable: "Trainees",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_CourseSubject_CourseSubjectCode",
                table: "CourseSchedule",
                column: "CourseSubjectCode",
                principalTable: "CourseSubject",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseHour_Trainees_TraineesGuid",
                table: "CourseHour");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseSchedule_CourseSubject_CourseSubjectCode",
                table: "CourseSchedule");

            migrationBuilder.DropTable(
                name: "ClassGradeCourseScheduleMapping");

            migrationBuilder.DropTable(
                name: "CourseHourCourseScheduleMapping");

            migrationBuilder.DropIndex(
                name: "IX_CourseSchedule_CourseSubjectCode",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "CourseSubjectCode",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "CourseSubjectGuid",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "EndMonth",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "StartDay",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                table: "CourseSchedule");

            migrationBuilder.DropColumn(
                name: "IsSpecial",
                table: "ClassGrade");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassGradeGuid",
                table: "CourseSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CourseSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_ClassGradeGuid",
                table: "CourseSchedule",
                column: "ClassGradeGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseHour_Trainees_TraineesGuid",
                table: "CourseHour",
                column: "TraineesGuid",
                principalTable: "Trainees",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseSchedule_ClassGrade_ClassGradeGuid",
                table: "CourseSchedule",
                column: "ClassGradeGuid",
                principalTable: "ClassGrade",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
