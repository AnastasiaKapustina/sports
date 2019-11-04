using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SportId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDualPlayer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTypes_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SportId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SportId = table.Column<Guid>(nullable: false),
                    Team1Id = table.Column<Guid>(nullable: false),
                    Team2Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_Team1Id",
                        column: x => x.Team1Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Teams_Team2Id",
                        column: x => x.Team2Id,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    EventTypeId = table.Column<Guid>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Player1Id = table.Column<Guid>(nullable: true),
                    Player2Id = table.Column<Guid>(nullable: true),
                    PlayerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Players_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Players_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("2d247466-f4c9-4827-9d55-ccb6794f4dbf"), "Футбол" });

            migrationBuilder.InsertData(
                table: "Sports",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("f3e01830-f90d-4225-bda3-51cf6eefb740"), "Баскетбол" });

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "IsDualPlayer", "Name", "SportId" },
                values: new object[,]
                {
                    { new Guid("27afac17-4076-42c9-8c4e-05ad35c98c9c"), false, "Гол", new Guid("2d247466-f4c9-4827-9d55-ccb6794f4dbf") },
                    { new Guid("5d42f16d-7dee-4ba2-a652-c26914c4d8c7"), true, "Пас", new Guid("2d247466-f4c9-4827-9d55-ccb6794f4dbf") },
                    { new Guid("73a70081-3824-46b0-987f-da268547aab5"), false, "Пенальти", new Guid("2d247466-f4c9-4827-9d55-ccb6794f4dbf") },
                    { new Guid("3958f90e-a6b3-4322-bd11-5d03fe40deb8"), false, "Карточка", new Guid("2d247466-f4c9-4827-9d55-ccb6794f4dbf") },
                    { new Guid("757f68ce-e92f-4376-959f-85c1b6c8678c"), false, "Гол", new Guid("f3e01830-f90d-4225-bda3-51cf6eefb740") },
                    { new Guid("8cb53f7e-ec21-4dd8-8739-3188d71fc144"), true, "Пас", new Guid("f3e01830-f90d-4225-bda3-51cf6eefb740") },
                    { new Guid("a7953328-224b-42f6-88a7-3b422b25acce"), false, "Штрафной", new Guid("f3e01830-f90d-4225-bda3-51cf6eefb740") },
                    { new Guid("1869553d-53e2-4718-8265-639ef0ae4a5b"), false, "Нарушение", new Guid("f3e01830-f90d-4225-bda3-51cf6eefb740") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId",
                table: "Events",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GameId",
                table: "Events",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Player1Id",
                table: "Events",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Player2Id",
                table: "Events",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PlayerId",
                table: "Events",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTypes_SportId",
                table: "EventTypes",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_SportId",
                table: "Games",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team1Id",
                table: "Games",
                column: "Team1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Team2Id",
                table: "Games",
                column: "Team2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SportId",
                table: "Teams",
                column: "SportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}
