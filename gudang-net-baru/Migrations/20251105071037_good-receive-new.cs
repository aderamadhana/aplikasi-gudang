using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gudang_net_baru.Migrations
{
    /// <inheritdoc />
    public partial class goodreceivenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LokasiStagingName",
                table: "GoodReceiveDetail",
                newName: "StatusQC");

            migrationBuilder.RenameColumn(
                name: "LokasiStagingId",
                table: "GoodReceiveDetail",
                newName: "Expiry");

            migrationBuilder.RenameColumn(
                name: "StatusGrn",
                table: "GoodReceive",
                newName: "StatusGr");

            migrationBuilder.AddColumn<int>(
                name: "QtyOrder",
                table: "GoodReceiveDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LokasiId",
                table: "GoodReceive",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LokasiName",
                table: "GoodReceive",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyOrder",
                table: "GoodReceiveDetail");

            migrationBuilder.DropColumn(
                name: "LokasiId",
                table: "GoodReceive");

            migrationBuilder.DropColumn(
                name: "LokasiName",
                table: "GoodReceive");

            migrationBuilder.RenameColumn(
                name: "StatusQC",
                table: "GoodReceiveDetail",
                newName: "LokasiStagingName");

            migrationBuilder.RenameColumn(
                name: "Expiry",
                table: "GoodReceiveDetail",
                newName: "LokasiStagingId");

            migrationBuilder.RenameColumn(
                name: "StatusGr",
                table: "GoodReceive",
                newName: "StatusGrn");
        }
    }
}
