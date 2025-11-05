using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gudang_net_baru.Migrations
{
    /// <inheritdoc />
    public partial class goodreceive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodReceive",
                columns: table => new
                {
                    GrnId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GrnNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TanggalTerima = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusGrn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodReceive", x => x.GrnId);
                });

            migrationBuilder.CreateTable(
                name: "GoodReceiveDetail",
                columns: table => new
                {
                    GrnDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtyReceived = table.Column<int>(type: "int", nullable: true),
                    LokasiStagingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LokasiStagingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoodReceiveEntityGrnId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodReceiveDetail", x => x.GrnDetailId);
                    table.ForeignKey(
                        name: "FK_GoodReceiveDetail_GoodReceive_GoodReceiveEntityGrnId",
                        column: x => x.GoodReceiveEntityGrnId,
                        principalTable: "GoodReceive",
                        principalColumn: "GrnId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceiveDetail_GoodReceiveEntityGrnId",
                table: "GoodReceiveDetail",
                column: "GoodReceiveEntityGrnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodReceiveDetail");

            migrationBuilder.DropTable(
                name: "GoodReceive");
        }
    }
}
