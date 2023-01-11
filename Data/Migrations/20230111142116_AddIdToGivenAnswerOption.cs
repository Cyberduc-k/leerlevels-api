using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddIdToGivenAnswerOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GivenAnswerOption_AnswerOptions_AnswerId",
                table: "GivenAnswerOption");

            migrationBuilder.DropForeignKey(
                name: "FK_GivenAnswerOption_McqProgress_McqProgressId",
                table: "GivenAnswerOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GivenAnswerOption",
                table: "GivenAnswerOption");

            migrationBuilder.RenameTable(
                name: "GivenAnswerOption",
                newName: "GivenAnswerOptions");

            migrationBuilder.RenameIndex(
                name: "IX_GivenAnswerOption_McqProgressId",
                table: "GivenAnswerOptions",
                newName: "IX_GivenAnswerOptions_McqProgressId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GivenAnswerOptions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GivenAnswerOptions",
                table: "GivenAnswerOptions",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "LastLogin",
                value: new DateTime(2023, 1, 11, 15, 21, 16, 222, DateTimeKind.Local).AddTicks(2531));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                column: "LastLogin",
                value: new DateTime(2023, 1, 11, 15, 21, 16, 222, DateTimeKind.Local).AddTicks(2565));

            migrationBuilder.CreateIndex(
                name: "IX_GivenAnswerOptions_AnswerId",
                table: "GivenAnswerOptions",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GivenAnswerOptions_AnswerOptions_AnswerId",
                table: "GivenAnswerOptions",
                column: "AnswerId",
                principalTable: "AnswerOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GivenAnswerOptions_McqProgress_McqProgressId",
                table: "GivenAnswerOptions",
                column: "McqProgressId",
                principalTable: "McqProgress",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GivenAnswerOptions_AnswerOptions_AnswerId",
                table: "GivenAnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_GivenAnswerOptions_McqProgress_McqProgressId",
                table: "GivenAnswerOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GivenAnswerOptions",
                table: "GivenAnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_GivenAnswerOptions_AnswerId",
                table: "GivenAnswerOptions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GivenAnswerOptions");

            migrationBuilder.RenameTable(
                name: "GivenAnswerOptions",
                newName: "GivenAnswerOption");

            migrationBuilder.RenameIndex(
                name: "IX_GivenAnswerOptions_McqProgressId",
                table: "GivenAnswerOption",
                newName: "IX_GivenAnswerOption_McqProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GivenAnswerOption",
                table: "GivenAnswerOption",
                columns: new[] { "AnswerId", "AnswerKind" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "LastLogin",
                value: new DateTime(2022, 11, 26, 21, 38, 29, 149, DateTimeKind.Local).AddTicks(3512));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                column: "LastLogin",
                value: new DateTime(2022, 11, 26, 21, 38, 29, 149, DateTimeKind.Local).AddTicks(3542));

            migrationBuilder.AddForeignKey(
                name: "FK_GivenAnswerOption_AnswerOptions_AnswerId",
                table: "GivenAnswerOption",
                column: "AnswerId",
                principalTable: "AnswerOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GivenAnswerOption_McqProgress_McqProgressId",
                table: "GivenAnswerOption",
                column: "McqProgressId",
                principalTable: "McqProgress",
                principalColumn: "Id");
        }
    }
}
