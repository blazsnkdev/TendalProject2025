using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TendalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblVenta_TblCliente_ClienteId",
                table: "TblVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_TblVenta_TblPedido_PedidoId",
                table: "TblVenta");

            migrationBuilder.DropTable(
                name: "TblDetalleVenta");

            migrationBuilder.DropColumn(
                name: "IGV",
                table: "TblVenta");

            migrationBuilder.DropColumn(
                name: "ImporteTotal",
                table: "TblVenta");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "TblVenta");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClienteId",
                table: "TblVenta",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEntrega",
                table: "TblPedido",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEnvio",
                table: "TblPedido",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "TblPedido",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Igv",
                table: "TblPedido",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "TblPedido",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "TblPedido",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_TblVenta_TblCliente_ClienteId",
                table: "TblVenta",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblVenta_TblPedido_PedidoId",
                table: "TblVenta",
                column: "PedidoId",
                principalTable: "TblPedido",
                principalColumn: "PedidoId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblVenta_TblCliente_ClienteId",
                table: "TblVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_TblVenta_TblPedido_PedidoId",
                table: "TblVenta");

            migrationBuilder.DropColumn(
                name: "FechaEntrega",
                table: "TblPedido");

            migrationBuilder.DropColumn(
                name: "FechaEnvio",
                table: "TblPedido");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "TblPedido");

            migrationBuilder.DropColumn(
                name: "Igv",
                table: "TblPedido");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "TblPedido");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "TblPedido");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClienteId",
                table: "TblVenta",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "IGV",
                table: "TblVenta",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ImporteTotal",
                table: "TblVenta",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "TblVenta",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TblDetalleVenta",
                columns: table => new
                {
                    DetalleVentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticuloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDetalleVenta", x => x.DetalleVentaId);
                    table.ForeignKey(
                        name: "FK_TblDetalleVenta_TblArticulo_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "TblArticulo",
                        principalColumn: "ArticuloId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblDetalleVenta_TblVenta_VentaId",
                        column: x => x.VentaId,
                        principalTable: "TblVenta",
                        principalColumn: "VentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblDetalleVenta_ArticuloId",
                table: "TblDetalleVenta",
                column: "ArticuloId");

            migrationBuilder.CreateIndex(
                name: "IX_TblDetalleVenta_VentaId",
                table: "TblDetalleVenta",
                column: "VentaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblVenta_TblCliente_ClienteId",
                table: "TblVenta",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TblVenta_TblPedido_PedidoId",
                table: "TblVenta",
                column: "PedidoId",
                principalTable: "TblPedido",
                principalColumn: "PedidoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
