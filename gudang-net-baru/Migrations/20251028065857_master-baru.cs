using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gudang_net_baru.Migrations
{
    /// <inheritdoc />
    public partial class masterbaru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Box",
                table: "MasterUnitMeasure");

            migrationBuilder.DropColumn(
                name: "Carton",
                table: "MasterUnitMeasure");

            migrationBuilder.DropColumn(
                name: "Ea",
                table: "MasterUnitMeasure");

            migrationBuilder.DropColumn(
                name: "Pallet",
                table: "MasterUnitMeasure");

            migrationBuilder.RenameColumn(
                name: "Transfer",
                table: "MasterReasonCode",
                newName: "ReasonCode");

            migrationBuilder.RenameColumn(
                name: "Return",
                table: "MasterReasonCode",
                newName: "Kategori");

            migrationBuilder.RenameColumn(
                name: "Adjustment",
                table: "MasterReasonCode",
                newName: "Deskripsi");

            migrationBuilder.AlterColumn<string>(
                name: "Conversion",
                table: "MasterUnitMeasure",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deskripsi",
                table: "MasterUnitMeasure",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deskripsi",
                table: "MasterUnitMeasure");

            migrationBuilder.RenameColumn(
                name: "ReasonCode",
                table: "MasterReasonCode",
                newName: "Transfer");

            migrationBuilder.RenameColumn(
                name: "Kategori",
                table: "MasterReasonCode",
                newName: "Return");

            migrationBuilder.RenameColumn(
                name: "Deskripsi",
                table: "MasterReasonCode",
                newName: "Adjustment");

            migrationBuilder.AlterColumn<int>(
                name: "Conversion",
                table: "MasterUnitMeasure",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Box",
                table: "MasterUnitMeasure",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Carton",
                table: "MasterUnitMeasure",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ea",
                table: "MasterUnitMeasure",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pallet",
                table: "MasterUnitMeasure",
                type: "int",
                nullable: true);
        }
    }
}
