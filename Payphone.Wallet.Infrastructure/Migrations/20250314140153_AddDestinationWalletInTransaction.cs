using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayphoneWallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDestinationWalletInTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationWalletId",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationWalletId",
                table: "Transactions");
        }
    }
}
