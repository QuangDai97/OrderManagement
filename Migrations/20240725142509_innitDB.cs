using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Migrations
{
    /// <inheritdoc />
    public partial class innitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderNo = table.Column<string>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeptCode = table.Column<string>(type: "TEXT", nullable: false),
                    CustCode = table.Column<string>(type: "TEXT", nullable: false),
                    CustSubNo = table.Column<int>(type: "INTEGER", nullable: true),
                    EmpCode = table.Column<string>(type: "TEXT", nullable: false),
                    RequiredDate = table.Column<string>(type: "TEXT", nullable: true),
                    CustOrderNo = table.Column<string>(type: "TEXT", nullable: true),
                    WhCode = table.Column<string>(type: "TEXT", nullable: false),
                    CmpTax = table.Column<int>(type: "INTEGER", nullable: false),
                    SlipComment = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updater = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderNo);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderNo = table.Column<string>(type: "TEXT", nullable: false),
                    SoRowNo = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdCode = table.Column<string>(type: "TEXT", nullable: false),
                    ProdName = table.Column<string>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CmpTaxRate = table.Column<int>(type: "INTEGER", nullable: true),
                    ReserveQty = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryOrderQty = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveredQty = table.Column<int>(type: "INTEGER", nullable: false),
                    CompleteFlg = table.Column<int>(type: "INTEGER", nullable: false),
                    Discount = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryDate = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updater = table.Column<string>(type: "TEXT", nullable: true),
                    OrderNo1 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => new { x.OrderNo, x.SoRowNo });
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderNo1",
                        column: x => x.OrderNo1,
                        principalTable: "Orders",
                        principalColumn: "OrderNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderNo1",
                table: "OrderDetails",
                column: "OrderNo1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
