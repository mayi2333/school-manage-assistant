using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 新增试听表和微信模板消息表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditionCourse",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    TraineesGuid = table.Column<Guid>(nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ClassGradeGuid = table.Column<Guid>(nullable: true),
                    TeacherAttenceGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditionCourse", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_AuditionCourse_ClassGrade_ClassGradeGuid",
                        column: x => x.ClassGradeGuid,
                        principalTable: "ClassGrade",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditionCourse_CourseSubject_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "CourseSubject",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditionCourse_TeacherAttence_TeacherAttenceGuid",
                        column: x => x.TeacherAttenceGuid,
                        principalTable: "TeacherAttence",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditionCourse_Trainees_TraineesGuid",
                        column: x => x.TraineesGuid,
                        principalTable: "Trainees",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateMsg",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    ToUser = table.Column<string>(nullable: true),
                    TemplateId = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    MsgId = table.Column<string>(nullable: true),
                    SendNum = table.Column<int>(nullable: false),
                    SendStatus = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateMsg", x => x.Guid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditionCourse_ClassGradeGuid",
                table: "AuditionCourse",
                column: "ClassGradeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_AuditionCourse_CourseCode",
                table: "AuditionCourse",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_AuditionCourse_TeacherAttenceGuid",
                table: "AuditionCourse",
                column: "TeacherAttenceGuid");

            migrationBuilder.CreateIndex(
                name: "IX_AuditionCourse_TraineesGuid",
                table: "AuditionCourse",
                column: "TraineesGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditionCourse");

            migrationBuilder.DropTable(
                name: "TemplateMsg");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Customer");
        }
    }
}
