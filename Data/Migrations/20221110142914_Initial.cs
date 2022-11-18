using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationType = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetExplanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YoutubeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastDeviceHandle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShareCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mcqs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowRandom = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mcqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mcqs_Targets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Targets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => new { x.UserId, x.ItemId, x.Type });
                    table.ForeignKey(
                        name: "FK_Bookmarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forums_Users_FromId",
                        column: x => x.FromId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TargetId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetProgress_Targets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Targets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TargetProgress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SetTarget",
                columns: table => new
                {
                    SetsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TargetsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTarget", x => new { x.SetsId, x.TargetsId });
                    table.ForeignKey(
                        name: "FK_SetTarget_Sets_SetsId",
                        column: x => x.SetsId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetTarget_Targets_TargetsId",
                        column: x => x.TargetsId,
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetUser",
                columns: table => new
                {
                    SetsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetUser", x => new { x.SetsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SetUser_Sets_SetsId",
                        column: x => x.SetsId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    McqId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Mcqs_McqId",
                        column: x => x.McqId,
                        principalTable: "Mcqs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upvotes = table.Column<int>(type: "int", nullable: false),
                    ForumId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replies_Forums_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Replies_Users_FromId",
                        column: x => x.FromId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "McqProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    McqId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnswerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnswerKind = table.Column<int>(type: "int", nullable: true),
                    TargetProgressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_McqProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_McqProgress_AnswerOptions_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "AnswerOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_McqProgress_Mcqs_McqId",
                        column: x => x.McqId,
                        principalTable: "Mcqs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_McqProgress_TargetProgress_TargetProgressId",
                        column: x => x.TargetProgressId,
                        principalTable: "TargetProgress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_McqProgress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "EducationType", "Name", "SchoolYear", "Subject" },
                values: new object[] { "DRWA-KCMN-PXYB-ZLQU", 1, "Inholland", 6, "Programming" });

            migrationBuilder.InsertData(
                table: "Targets",
                columns: new[] { "Id", "Description", "ImageUrl", "Label", "TargetExplanation", "YoutubeId" },
                values: new object[] { "1", "Je kan in eigen woorden uitleggen welk effect lading kan hebben.", "https://s3-us-west-2.amazonaws.com/leerlevels/slide_pngs/2.png", "Lading concept", "Lading is een eigenschap die bepaalt hoe een deeltje wordt beïnvloed door een elektrisch of magnetisch veld.", "0ouf-xbz7_o" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsActive", "LastDeviceHandle", "LastLogin", "LastName", "Password", "Role", "ShareCode", "UserName" },
                values: new object[,]
                {
                    { "1", "JohnDoe@gmail.com", "John", true, "11", new DateTime(2022, 10, 5, 13, 27, 0, 0, DateTimeKind.Unspecified), "Doe", "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$10000$xoUFLA1yQKZA/wvfJ9aBNPAJbbUY65QLhOeNeUA+ASwM5GjK", 0, "DTRY-WQER-PIGU-VNSA", "JohnD#1" },
                    { "2", "MarySue@gmail.com", "Mary", true, "22", new DateTime(2022, 11, 10, 15, 29, 14, 691, DateTimeKind.Local).AddTicks(66), "Sue", "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$10000$ZR9AMoHqh69WDC8SbEqMFwl2ERkrSDc62BFdt38Sx1tRaE5h", 1, "RIBN-QWOR-DCPL-AXCU", "MarySue#22" },
                    { "3", "Admin@gmail.com", "Admin", true, "33", new DateTime(2022, 11, 10, 15, 29, 14, 691, DateTimeKind.Local).AddTicks(102), "Admin", "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCsYBxHvBQzxYHGy5/JEpZvez3Gbw/QINZbNE4fpp9s5xqJT4KV4Q+aGWbWcp0CwS/8eyG94kBg/sFRc6umG88n1VkFZXJ0GFvvZ7TfC5NXJLpKs06Oebki4dp6ZL2re+WV+3JFfi9W+KtjUKDhhgt7haPkzFrK7LGpOjIvr/f0jQIDAQAB$10000$qcJ3B566KbRg/qPdPEjwwwpVxtE/T1pMXbLWNiK2JJe/XK1V", 2, "RIBN-QWOR-DCPL-AXCV", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Mcqs",
                columns: new[] { "Id", "AllowRandom", "Explanation", "QuestionText", "TargetId" },
                values: new object[] { "1", true, "Voorwerpen met gelijksoortige lading (beide positief of beide negatief) stoten elkaar af. Voorwerpen met tegengestelde lading (een positief, een negatief) trekken elkaar aan.", "Wat kun je zeggen over de lading van twee voorwerpen die elkaar aantrekken?", "1" });

            migrationBuilder.InsertData(
                table: "AnswerOptions",
                columns: new[] { "Id", "Index", "IsCorrect", "McqId", "Text" },
                values: new object[,]
                {
                    { "1", 0, false, "1", "Beide voorwerpen zijn negatief geladen" },
                    { "2", 1, false, "1", "Beide voorwerpen zijn positief geladen" },
                    { "3", 2, false, "1", "Beide voorwerpen zijn neutraal geladen" },
                    { "4", 3, true, "1", "De lading is tegengesteld" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_McqId",
                table: "AnswerOptions",
                column: "McqId");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_FromId",
                table: "Forums",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersId",
                table: "GroupUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_McqProgress_AnswerId",
                table: "McqProgress",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_McqProgress_McqId",
                table: "McqProgress",
                column: "McqId");

            migrationBuilder.CreateIndex(
                name: "IX_McqProgress_TargetProgressId",
                table: "McqProgress",
                column: "TargetProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_McqProgress_UserId",
                table: "McqProgress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mcqs_TargetId",
                table: "Mcqs",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ForumId",
                table: "Replies",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_FromId",
                table: "Replies",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_GroupId",
                table: "Sets",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SetTarget_TargetsId",
                table: "SetTarget",
                column: "TargetsId");

            migrationBuilder.CreateIndex(
                name: "IX_SetUser_UsersId",
                table: "SetUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetProgress_TargetId",
                table: "TargetProgress",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetProgress_UserId",
                table: "TargetProgress",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "McqProgress");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropTable(
                name: "SetTarget");

            migrationBuilder.DropTable(
                name: "SetUser");

            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "TargetProgress");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Mcqs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Targets");
        }
    }
}
