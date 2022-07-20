using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 新增客户表和学员表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    WxOpenid = table.Column<string>(type: "varchar(50)", nullable: false),
                    WxUnionid = table.Column<string>(type: "varchar(50)", nullable: true),
                    WxNickname = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WxSex = table.Column<int>(nullable: false),
                    WxProvince = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WxCity = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WxCountry = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WxHeadimgurl = table.Column<string>(type: "varchar(500)", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Trainees",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Telephone = table.Column<string>(type: "varchar(50)", nullable: false),
                    Address = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    CustomerGuid = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainees", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Trainees_Customer_CustomerGuid",
                        column: x => x.CustomerGuid,
                        principalTable: "Customer",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_CustomerGuid",
                table: "Trainees",
                column: "CustomerGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trainees");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
