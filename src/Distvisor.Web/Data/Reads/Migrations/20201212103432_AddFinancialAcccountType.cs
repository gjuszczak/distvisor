using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Reads.Migrations
{
    public partial class AddFinancialAcccountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "FinancialAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "FinancialAccounts");
        }
    }
}
