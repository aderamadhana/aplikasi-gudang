using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gudang_net_baru.Migrations
{
    /// <inheritdoc />
    public partial class purchaseorderupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitMeasureId",
                table: "PurchaseOrderDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitMeasureId",
                table: "PurchaseOrderDetail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
