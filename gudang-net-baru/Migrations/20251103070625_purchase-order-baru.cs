using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gudang_net_baru.Migrations
{
    /// <inheritdoc />
    public partial class purchaseorderbaru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PoNumber",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PoNumber",
                table: "PurchaseOrder");
        }
    }
}
