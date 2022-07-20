using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DncZeus.Api.Migrations
{
    public partial class 新增人脸特征记录表 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FaceFeatureGuid",
                table: "Trainees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdCardBindInfo",
                table: "Trainees",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FaceFeatureGuid",
                table: "Teacher",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdCardBindInfo",
                table: "Teacher",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FaceFeature",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    FaceEncodes = table.Column<string>(type: "varchar(3000)", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceFeature", x => x.Guid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_FaceFeatureGuid",
                table: "Trainees",
                column: "FaceFeatureGuid",
                unique: true,
                filter: "[FaceFeatureGuid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_FaceFeatureGuid",
                table: "Teacher",
                column: "FaceFeatureGuid",
                unique: true,
                filter: "[FaceFeatureGuid] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_FaceFeature_FaceFeatureGuid",
                table: "Teacher",
                column: "FaceFeatureGuid",
                principalTable: "FaceFeature",
                principalColumn: "Guid",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_FaceFeature_FaceFeatureGuid",
                table: "Trainees",
                column: "FaceFeatureGuid",
                principalTable: "FaceFeature",
                principalColumn: "Guid",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_FaceFeature_FaceFeatureGuid",
                table: "Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_FaceFeature_FaceFeatureGuid",
                table: "Trainees");

            migrationBuilder.DropTable(
                name: "FaceFeature");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_FaceFeatureGuid",
                table: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_FaceFeatureGuid",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "FaceFeatureGuid",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "IdCardBindInfo",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "FaceFeatureGuid",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "IdCardBindInfo",
                table: "Teacher");
        }
    }
}
