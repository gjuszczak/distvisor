using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Migrations
{
    public partial class v10_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Redirections",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Redirections", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Redirections");
        }
    }
}
