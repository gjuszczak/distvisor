using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Reads.Migrations
{
    public partial class AddHomeboxTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeboxTriggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxTriggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxTriggerActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TriggerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastExecutedActionNumber = table.Column<int>(type: "integer", nullable: true),
                    LastExecutedActionMinDelayMs = table.Column<int>(type: "integer", nullable: true),
                    LastExecutedActionMaxDelayMs = table.Column<int>(type: "integer", nullable: true),
                    IsDeviceOnline = table.Column<bool>(type: "boolean", nullable: true),
                    IsDeviceOn = table.Column<bool>(type: "boolean", nullable: true),
                    Payload = table.Column<JsonElement>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxTriggerActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeboxTriggerActions_HomeboxTriggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "HomeboxTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxTriggerSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TriggerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    MatchParam = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxTriggerSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeboxTriggerSources_HomeboxTriggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "HomeboxTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxTriggerTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TriggerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceIdentifier = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxTriggerTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeboxTriggerTargets_HomeboxTriggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "HomeboxTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeboxTriggerActions_TriggerId",
                table: "HomeboxTriggerActions",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeboxTriggerSources_TriggerId",
                table: "HomeboxTriggerSources",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeboxTriggerTargets_TriggerId",
                table: "HomeboxTriggerTargets",
                column: "TriggerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeboxTriggerActions");

            migrationBuilder.DropTable(
                name: "HomeboxTriggerSources");

            migrationBuilder.DropTable(
                name: "HomeboxTriggerTargets");

            migrationBuilder.DropTable(
                name: "HomeboxTriggers");
        }
    }
}
