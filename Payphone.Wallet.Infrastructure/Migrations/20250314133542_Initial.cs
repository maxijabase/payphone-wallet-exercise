using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayphoneWallet.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Wallets",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                DocumentId = table.Column<string>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATE()"),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATE()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Wallets", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                WalletId = table.Column<int>(type: "INTEGER", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Type = table.Column<int>(type: "INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATE()"),
                UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATE()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Transactions_Wallets_WalletId",
                    column: x => x.WalletId,
                    principalTable: "Wallets",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_WalletId",
            table: "Transactions",
            column: "WalletId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Transactions");

        migrationBuilder.DropTable(
            name: "Wallets");
    }
}
