using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations;

public partial class AddNameToSet : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Sets",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: "1",
            column: "Password",
            value: "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$AQAAAAEAACcQAAAAEC5ynu4doF+Ndp8v1pvxkxfUseWCAZ6095V/GCQqA4L9A1hi0t/1gBLf8atq8P60Aw==");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: "2",
            columns: new[] { "LastLogin", "Password" },
            values: new object[] { new DateTime(2022, 11, 17, 18, 35, 52, 509, DateTimeKind.Local).AddTicks(76), "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$AQAAAAEAACcQAAAAEPvUCXnvR1fic6e98jZnZqyD2GUauqKwWnEVsMu5AGbm1PggvwocdtgxW/IIfeZh8g==" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: "3",
            columns: new[] { "LastLogin", "Password" },
            values: new object[] { new DateTime(2022, 11, 17, 18, 35, 52, 509, DateTimeKind.Local).AddTicks(108), "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$AQAAAAEAACcQAAAAEKqxLer0zNhevpkLr0sqS4tClT3Gnn77qx+/4FcGAWE/F7AqSPpig8dL7s09znzUzQ==" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "Sets");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: "1",
            column: "Password",
            value: "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$10000$xoUFLA1yQKZA/wvfJ9aBNPAJbbUY65QLhOeNeUA+ASwM5GjK");

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: "2",
            columns: new[] { "LastLogin", "Password" },
            values: new object[] { new DateTime(2022, 11, 10, 15, 29, 14, 691, DateTimeKind.Local).AddTicks(66), "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$10000$ZR9AMoHqh69WDC8SbEqMFwl2ERkrSDc62BFdt38Sx1tRaE5h" });

        migrationBuilder.UpdateData(
            table: "Users",
            keyColumn: "Id",
            keyValue: "3",
            columns: new[] { "LastLogin", "Password" },
            values: new object[] { new DateTime(2022, 11, 10, 15, 29, 14, 691, DateTimeKind.Local).AddTicks(102), "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$10000$qcJ3B566KbRg/qPdPEjwwwpVxtE/T1pMXbLWNiK2JJe/XK1V" });
    }
}
