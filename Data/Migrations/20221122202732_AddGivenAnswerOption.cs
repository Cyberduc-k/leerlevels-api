using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddGivenAnswerOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_McqProgress_AnswerOptions_AnswerId",
                table: "McqProgress");

            migrationBuilder.DropIndex(
                name: "IX_McqProgress_AnswerId",
                table: "McqProgress");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "McqProgress");

            migrationBuilder.DropColumn(
                name: "AnswerKind",
                table: "McqProgress");

            migrationBuilder.CreateTable(
                name: "GivenAnswerOption",
                columns: table => new
                {
                    AnswerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnswerKind = table.Column<int>(type: "int", nullable: false),
                    McqProgressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GivenAnswerOption", x => new { x.AnswerId, x.AnswerKind });
                    table.ForeignKey(
                        name: "FK_GivenAnswerOption_AnswerOptions_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "AnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GivenAnswerOption_McqProgress_McqProgressId",
                        column: x => x.McqProgressId,
                        principalTable: "McqProgress",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "LastLogin",
                value: new DateTime(2022, 11, 22, 21, 27, 31, 806, DateTimeKind.Local).AddTicks(2199));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                column: "LastLogin",
                value: new DateTime(2022, 11, 22, 21, 27, 31, 806, DateTimeKind.Local).AddTicks(2234));

            migrationBuilder.CreateIndex(
                name: "IX_GivenAnswerOption_McqProgressId",
                table: "GivenAnswerOption",
                column: "McqProgressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GivenAnswerOption");

            migrationBuilder.AddColumn<string>(
                name: "AnswerId",
                table: "McqProgress",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnswerKind",
                table: "McqProgress",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "LastLogin",
                value: new DateTime(2022, 11, 17, 18, 35, 52, 509, DateTimeKind.Local).AddTicks(76));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                column: "LastLogin",
                value: new DateTime(2022, 11, 17, 18, 35, 52, 509, DateTimeKind.Local).AddTicks(108));

            migrationBuilder.CreateIndex(
                name: "IX_McqProgress_AnswerId",
                table: "McqProgress",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_McqProgress_AnswerOptions_AnswerId",
                table: "McqProgress",
                column: "AnswerId",
                principalTable: "AnswerOptions",
                principalColumn: "Id");
        }
    }
}
