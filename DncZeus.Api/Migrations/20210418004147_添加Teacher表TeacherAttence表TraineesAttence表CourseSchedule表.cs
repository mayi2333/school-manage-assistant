using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 添加Teacher表TeacherAttence表TraineesAttence表CourseSchedule表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Telephone = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "CourseSchedule",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<int>(nullable: false),
                    ClassGradeGuid = table.Column<Guid>(nullable: false),
                    TeacherGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSchedule", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_CourseSchedule_ClassGrade_ClassGradeGuid",
                        column: x => x.ClassGradeGuid,
                        principalTable: "ClassGrade",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseSchedule_Teacher_TeacherGuid",
                        column: x => x.TeacherGuid,
                        principalTable: "Teacher",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherAttence",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsAttend = table.Column<int>(nullable: false),
                    IsSubstitute = table.Column<int>(nullable: false),
                    CreatedByUserGuid = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedByUserName = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(nullable: true),
                    TeacherGuid = table.Column<Guid>(nullable: false),
                    CourseScheduleGuid = table.Column<Guid>(nullable: false),
                    ParentGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAttence", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_TeacherAttence_CourseSchedule_CourseScheduleGuid",
                        column: x => x.CourseScheduleGuid,
                        principalTable: "CourseSchedule",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherAttence_TeacherAttence_ParentGuid",
                        column: x => x.ParentGuid,
                        principalTable: "TeacherAttence",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherAttence_Teacher_TeacherGuid",
                        column: x => x.TeacherGuid,
                        principalTable: "Teacher",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TraineesAttence",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    IsAttend = table.Column<int>(nullable: false),
                    CreatedByUserGuid = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedByUserName = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedByUserGuid = table.Column<Guid>(nullable: true),
                    ModifiedByUserName = table.Column<string>(nullable: true),
                    CourseHourGuid = table.Column<Guid>(nullable: false),
                    CourseScheduleGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineesAttence", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_TraineesAttence_CourseHour_CourseHourGuid",
                        column: x => x.CourseHourGuid,
                        principalTable: "CourseHour",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TraineesAttence_CourseSchedule_CourseScheduleGuid",
                        column: x => x.CourseScheduleGuid,
                        principalTable: "CourseSchedule",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_ClassGradeGuid",
                table: "CourseSchedule",
                column: "ClassGradeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedule_TeacherGuid",
                table: "CourseSchedule",
                column: "TeacherGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAttence_CourseScheduleGuid",
                table: "TeacherAttence",
                column: "CourseScheduleGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAttence_ParentGuid",
                table: "TeacherAttence",
                column: "ParentGuid",
                unique: true,
                filter: "[ParentGuid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAttence_TeacherGuid",
                table: "TeacherAttence",
                column: "TeacherGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TraineesAttence_CourseHourGuid",
                table: "TraineesAttence",
                column: "CourseHourGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TraineesAttence_CourseScheduleGuid",
                table: "TraineesAttence",
                column: "CourseScheduleGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherAttence");

            migrationBuilder.DropTable(
                name: "TraineesAttence");

            migrationBuilder.DropTable(
                name: "CourseSchedule");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
