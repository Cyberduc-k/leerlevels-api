using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations;

public partial class AddLinks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Links",
            columns: table => new {
                FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ToId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_Links", x => new { x.FromId, x.ToId });
                table.ForeignKey(
                    name: "FK_Links_Targets_FromId",
                    column: x => x.FromId,
                    principalTable: "Targets",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Links_Targets_ToId",
                    column: x => x.ToId,
                    principalTable: "Targets",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.NoAction);
            });

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

        migrationBuilder.CreateIndex(
            name: "IX_Links_ToId",
            table: "Links",
            column: "ToId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Links");

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
    }
}
