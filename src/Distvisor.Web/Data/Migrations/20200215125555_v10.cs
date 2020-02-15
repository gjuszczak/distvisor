using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Migrations
{
    public partial class v010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyVault");

            migrationBuilder.CreateTable(
                name: "SecretsVault",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretsVault", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecretsVault");

            migrationBuilder.CreateTable(
                name: "KeyVault",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    KeyValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyVault", x => x.Id);
                });
        }
    }
}
