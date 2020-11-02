using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Distvisor.Web.Data.Reads.Migrations
{
    public partial class AddProcessedEmails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedEmails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueKey = table.Column<string>(nullable: true),
                    BodyMime = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedEmails");
        }
    }
}
