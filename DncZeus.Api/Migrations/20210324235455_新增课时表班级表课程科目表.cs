using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 新增课时表班级表课程科目表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Customer_CustomerGuid",
                table: "Trainees");

            migrationBuilder.CreateTable(
                name: "CourseSubject",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    ChargeType = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubject", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ClassGrade",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TotalPeople = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<int>(nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassGrade", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ClassGrade_CourseSubject_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "CourseSubject",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseHour",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Surplus = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    OperationLog = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TraineesGuid = table.Column<Guid>(nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ClassGradeGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseHour", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_CourseHour_ClassGrade_ClassGradeGuid",
                        column: x => x.ClassGradeGuid,
                        principalTable: "ClassGrade",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CourseHour_CourseSubject_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "CourseSubject",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseHour_Trainees_TraineesGuid",
                        column: x => x.TraineesGuid,
                        principalTable: "Trainees",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrade_CourseCode",
                table: "ClassGrade",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CourseHour_ClassGradeGuid",
                table: "CourseHour",
                column: "ClassGradeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CourseHour_CourseCode",
                table: "CourseHour",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_CourseHour_TraineesGuid",
                table: "CourseHour",
                column: "TraineesGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubject_Code",
                table: "CourseSubject",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Customer_CustomerGuid",
                table: "Trainees",
                column: "CustomerGuid",
                principalTable: "Customer",
                principalColumn: "Guid",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Customer_CustomerGuid",
                table: "Trainees");

            migrationBuilder.DropTable(
                name: "CourseHour");

            migrationBuilder.DropTable(
                name: "ClassGrade");

            migrationBuilder.DropTable(
                name: "CourseSubject");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Customer_CustomerGuid",
                table: "Trainees",
                column: "CustomerGuid",
                principalTable: "Customer",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
