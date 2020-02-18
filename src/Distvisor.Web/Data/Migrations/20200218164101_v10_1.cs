using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Migrations
{
    public partial class v10_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OAuthTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Issuer = table.Column<string>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    TokenType = table.Column<string>(nullable: true),
                    ExpiresIn = table.Column<DateTime>(nullable: false),
                    Scope = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OAuthTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OAuthTokens_UserId",
                table: "OAuthTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OAuthTokens");
        }
    }
}
