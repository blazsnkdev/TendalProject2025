using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TendalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateNoMeAcuerdoXD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carrito_TblCliente_ClienteId",
                table: "Carrito");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedido_Pedido_PedidoId",
                table: "DetallePedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedido_TblArticulo_ArticuloId",
                table: "DetallePedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVenta_TblArticulo_ArticuloId",
                table: "DetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVenta_Venta_VentaId",
                table: "DetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Carrito_CarritoId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_TblArticulo_ArticuloId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_TblCliente_ClienteId",
                table: "Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Venta_Pedido_PedidoId",
                table: "Venta");

            migrationBuilder.DropForeignKey(
                name: "FK_Venta_TblCliente_ClienteId",
                table: "Venta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venta",
                table: "Venta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "Item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleVenta",
                table: "DetalleVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetallePedido",
                table: "DetallePedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carrito",
                table: "Carrito");

            migrationBuilder.RenameTable(
                name: "Venta",
                newName: "TblVenta");

            migrationBuilder.RenameTable(
                name: "Pedido",
                newName: "TblPedido");

            migrationBuilder.RenameTable(
                name: "Item",
                newName: "TblItem");

            migrationBuilder.RenameTable(
                name: "DetalleVenta",
                newName: "TblDetalleVenta");

            migrationBuilder.RenameTable(
                name: "DetallePedido",
                newName: "TblDetallePedido");

            migrationBuilder.RenameTable(
                name: "Carrito",
                newName: "TblCarrito");

            migrationBuilder.RenameIndex(
                name: "IX_Venta_PedidoId",
                table: "TblVenta",
                newName: "IX_TblVenta_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_Venta_ClienteId",
                table: "TblVenta",
                newName: "IX_TblVenta_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedido_ClienteId",
                table: "TblPedido",
                newName: "IX_TblPedido_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_CarritoId",
                table: "TblItem",
                newName: "IX_TblItem_CarritoId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_ArticuloId",
                table: "TblItem",
                newName: "IX_TblItem_ArticuloId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleVenta_VentaId",
                table: "TblDetalleVenta",
                newName: "IX_TblDetalleVenta_VentaId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleVenta_ArticuloId",
                table: "TblDetalleVenta",
                newName: "IX_TblDetalleVenta_ArticuloId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallePedido_PedidoId",
                table: "TblDetallePedido",
                newName: "IX_TblDetallePedido_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallePedido_ArticuloId",
                table: "TblDetallePedido",
                newName: "IX_TblDetallePedido_ArticuloId");

            migrationBuilder.RenameIndex(
                name: "IX_Carrito_ClienteId",
                table: "TblCarrito",
                newName: "IX_TblCarrito_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblVenta",
                table: "TblVenta",
                column: "VentaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblPedido",
                table: "TblPedido",
                column: "PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblItem",
                table: "TblItem",
                column: "ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblDetalleVenta",
                table: "TblDetalleVenta",
                column: "DetalleVentaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblDetallePedido",
                table: "TblDetallePedido",
                column: "DetallePedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblCarrito",
                table: "TblCarrito",
                column: "CarritoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblCarrito_TblCliente_ClienteId",
                table: "TblCarrito",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblDetallePedido_TblArticulo_ArticuloId",
                table: "TblDetallePedido",
                column: "ArticuloId",
                principalTable: "TblArticulo",
                principalColumn: "ArticuloId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TblDetallePedido_TblPedido_PedidoId",
                table: "TblDetallePedido",
                column: "PedidoId",
                principalTable: "TblPedido",
                principalColumn: "PedidoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblDetalleVenta_TblArticulo_ArticuloId",
                table: "TblDetalleVenta",
                column: "ArticuloId",
                principalTable: "TblArticulo",
                principalColumn: "ArticuloId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TblDetalleVenta_TblVenta_VentaId",
                table: "TblDetalleVenta",
                column: "VentaId",
                principalTable: "TblVenta",
                principalColumn: "VentaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblItem_TblArticulo_ArticuloId",
                table: "TblItem",
                column: "ArticuloId",
                principalTable: "TblArticulo",
                principalColumn: "ArticuloId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TblItem_TblCarrito_CarritoId",
                table: "TblItem",
                column: "CarritoId",
                principalTable: "TblCarrito",
                principalColumn: "CarritoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblPedido_TblCliente_ClienteId",
                table: "TblPedido",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblCarrito_TblCliente_ClienteId",
                table: "TblCarrito");

            migrationBuilder.DropForeignKey(
                name: "FK_TblDetallePedido_TblArticulo_ArticuloId",
                table: "TblDetallePedido");

            migrationBuilder.DropForeignKey(
                name: "FK_TblDetallePedido_TblPedido_PedidoId",
                table: "TblDetallePedido");

            migrationBuilder.DropForeignKey(
                name: "FK_TblDetalleVenta_TblArticulo_ArticuloId",
                table: "TblDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_TblDetalleVenta_TblVenta_VentaId",
                table: "TblDetalleVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_TblItem_TblArticulo_ArticuloId",
                table: "TblItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TblItem_TblCarrito_CarritoId",
                table: "TblItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TblPedido_TblCliente_ClienteId",
                table: "TblPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_TblVenta_TblCliente_ClienteId",
                table: "TblVenta");

            migrationBuilder.DropForeignKey(
                name: "FK_TblVenta_TblPedido_PedidoId",
                table: "TblVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblVenta",
                table: "TblVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblPedido",
                table: "TblPedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblItem",
                table: "TblItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblDetalleVenta",
                table: "TblDetalleVenta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblDetallePedido",
                table: "TblDetallePedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblCarrito",
                table: "TblCarrito");

            migrationBuilder.RenameTable(
                name: "TblVenta",
                newName: "Venta");

            migrationBuilder.RenameTable(
                name: "TblPedido",
                newName: "Pedido");

            migrationBuilder.RenameTable(
                name: "TblItem",
                newName: "Item");

            migrationBuilder.RenameTable(
                name: "TblDetalleVenta",
                newName: "DetalleVenta");

            migrationBuilder.RenameTable(
                name: "TblDetallePedido",
                newName: "DetallePedido");

            migrationBuilder.RenameTable(
                name: "TblCarrito",
                newName: "Carrito");

            migrationBuilder.RenameIndex(
                name: "IX_TblVenta_PedidoId",
                table: "Venta",
                newName: "IX_Venta_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_TblVenta_ClienteId",
                table: "Venta",
                newName: "IX_Venta_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_TblPedido_ClienteId",
                table: "Pedido",
                newName: "IX_Pedido_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_TblItem_CarritoId",
                table: "Item",
                newName: "IX_Item_CarritoId");

            migrationBuilder.RenameIndex(
                name: "IX_TblItem_ArticuloId",
                table: "Item",
                newName: "IX_Item_ArticuloId");

            migrationBuilder.RenameIndex(
                name: "IX_TblDetalleVenta_VentaId",
                table: "DetalleVenta",
                newName: "IX_DetalleVenta_VentaId");

            migrationBuilder.RenameIndex(
                name: "IX_TblDetalleVenta_ArticuloId",
                table: "DetalleVenta",
                newName: "IX_DetalleVenta_ArticuloId");

            migrationBuilder.RenameIndex(
                name: "IX_TblDetallePedido_PedidoId",
                table: "DetallePedido",
                newName: "IX_DetallePedido_PedidoId");

            migrationBuilder.RenameIndex(
                name: "IX_TblDetallePedido_ArticuloId",
                table: "DetallePedido",
                newName: "IX_DetallePedido_ArticuloId");

            migrationBuilder.RenameIndex(
                name: "IX_TblCarrito_ClienteId",
                table: "Carrito",
                newName: "IX_Carrito_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venta",
                table: "Venta",
                column: "VentaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido",
                column: "PedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "Item",
                column: "ItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleVenta",
                table: "DetalleVenta",
                column: "DetalleVentaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetallePedido",
                table: "DetallePedido",
                column: "DetallePedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carrito",
                table: "Carrito",
                column: "CarritoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carrito_TblCliente_ClienteId",
                table: "Carrito",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedido_Pedido_PedidoId",
                table: "DetallePedido",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "PedidoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedido_TblArticulo_ArticuloId",
                table: "DetallePedido",
                column: "ArticuloId",
                principalTable: "TblArticulo",
                principalColumn: "ArticuloId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVenta_TblArticulo_ArticuloId",
                table: "DetalleVenta",
                column: "ArticuloId",
                principalTable: "TblArticulo",
                principalColumn: "ArticuloId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVenta_Venta_VentaId",
                table: "DetalleVenta",
                column: "VentaId",
                principalTable: "Venta",
                principalColumn: "VentaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Carrito_CarritoId",
                table: "Item",
                column: "CarritoId",
                principalTable: "Carrito",
                principalColumn: "CarritoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_TblArticulo_ArticuloId",
                table: "Item",
                column: "ArticuloId",
                principalTable: "TblArticulo",
                principalColumn: "ArticuloId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_TblCliente_ClienteId",
                table: "Pedido",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venta_Pedido_PedidoId",
                table: "Venta",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "PedidoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venta_TblCliente_ClienteId",
                table: "Venta",
                column: "ClienteId",
                principalTable: "TblCliente",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
