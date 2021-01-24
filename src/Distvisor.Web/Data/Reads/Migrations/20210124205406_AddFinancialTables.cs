using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Distvisor.Web.Data.Reads.Migrations
{
    public partial class AddFinancialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniqueKey = table.Column<string>(type: "text", nullable: true),
                    BodyMime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedEmails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccountPaycards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountPaycards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAccountPaycards_FinancialAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "FinancialAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccountTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeqNo = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    TransactionHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAccountTransactions_FinancialAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "FinancialAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountPaycards_AccountId",
                table: "FinancialAccountPaycards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountPaycards_Name",
                table: "FinancialAccountPaycards",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccounts_Number",
                table: "FinancialAccounts",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountTransactions_AccountId_SeqNo",
                table: "FinancialAccountTransactions",
                columns: new[] { "AccountId", "SeqNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountTransactions_TransactionHash",
                table: "FinancialAccountTransactions",
                column: "TransactionHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedEmails_UniqueKey",
                table: "ProcessedEmails",
                column: "UniqueKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialAccountPaycards");

            migrationBuilder.DropTable(
                name: "FinancialAccountTransactions");

            migrationBuilder.DropTable(
                name: "ProcessedEmails");

            migrationBuilder.DropTable(
                name: "FinancialAccounts");
        }
    }
}
