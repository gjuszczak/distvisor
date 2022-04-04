using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeStamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateType = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxDeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxDeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxGatewaySessionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxGatewaySessionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GatewayDeviceId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Header = table.Column<string>(type: "text", nullable: true),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    Params = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeboxDevices_HomeboxDeviceTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "HomeboxDeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HomeboxGatewaySessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    TokenGeneratedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeboxGatewaySessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeboxGatewaySessions_HomeboxGatewaySessionStatuses_Status~",
                        column: x => x.StatusId,
                        principalTable: "HomeboxGatewaySessionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeboxDevices_TypeId",
                table: "HomeboxDevices",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeboxGatewaySessions_StatusId",
                table: "HomeboxGatewaySessions",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "HomeboxDevices");

            migrationBuilder.DropTable(
                name: "HomeboxGatewaySessions");

            migrationBuilder.DropTable(
                name: "HomeboxDeviceTypes");

            migrationBuilder.DropTable(
                name: "HomeboxGatewaySessionStatuses");
        }
    }
}
