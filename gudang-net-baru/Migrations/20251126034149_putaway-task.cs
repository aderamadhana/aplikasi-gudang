using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gudang_net_baru.Migrations
{
    /// <inheritdoc />
    public partial class putawaytask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PutawayTask",
                columns: table => new
                {
                    PutawayId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PutawayNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoodReceiveId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PutawayTask", x => x.PutawayId);
                });

            migrationBuilder.CreateTable(
                name: "PutawayTaskDetail",
                columns: table => new
                {
                    PutawayDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PutawayId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrDetailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QtyToPutaway = table.Column<int>(type: "int", nullable: true),
                    FromLocationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToLocationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PutawayTaskEntityPutawayId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PutawayTaskDetail", x => x.PutawayDetailId);
                    table.ForeignKey(
                        name: "FK_PutawayTaskDetail_PutawayTask_PutawayTaskEntityPutawayId",
                        column: x => x.PutawayTaskEntityPutawayId,
                        principalTable: "PutawayTask",
                        principalColumn: "PutawayId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PutawayTaskDetail_PutawayTaskEntityPutawayId",
                table: "PutawayTaskDetail",
                column: "PutawayTaskEntityPutawayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PutawayTaskDetail");

            migrationBuilder.DropTable(
                name: "PutawayTask");
        }
    }
}
