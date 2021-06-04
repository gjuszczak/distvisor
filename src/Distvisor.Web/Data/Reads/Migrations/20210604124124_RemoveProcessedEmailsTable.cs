using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Reads.Migrations
{
    public partial class RemoveProcessedEmailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedEmails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BodyMime = table.Column<string>(type: "text", nullable: true),
                    UniqueKey = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedEmails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedEmails_UniqueKey",
                table: "ProcessedEmails",
                column: "UniqueKey",
                unique: true);
        }
    }
}
