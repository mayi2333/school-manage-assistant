using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 添加OperationLog表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationLog",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    OperationTime = table.Column<DateTime>(nullable: false),
                    OperationByUserName = table.Column<string>(nullable: true),
                    OperationByUserGuid = table.Column<Guid>(nullable: false),
                    MoudleName = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    ControllerName = table.Column<string>(nullable: true),
                    ActionName = table.Column<string>(nullable: true),
                    Parameter = table.Column<string>(nullable: true),
                    Descriptor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLog", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationLog");
        }
    }
}
