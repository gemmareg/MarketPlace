using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class paymentUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_PedidoId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PedidoId",
                table: "Payments",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "FechaPago",
                table: "Payments",
                newName: "PaymentDate");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PedidoId",
                table: "Payments",
                newName: "IX_Payments_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "Payments",
                newName: "FechaPago");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Payments",
                newName: "PedidoId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Payments",
                newName: "Monto");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                newName: "IX_Payments_PedidoId");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_PedidoId",
                table: "Payments",
                column: "PedidoId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
