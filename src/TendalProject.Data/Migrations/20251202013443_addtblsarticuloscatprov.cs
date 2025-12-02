using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TendalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class addtblsarticuloscatprov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblCategoria",
                columns: table => new
                {
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCategoria", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "TblProveedor",
                columns: table => new
                {
                    ProveedorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ruc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblProveedor", x => x.ProveedorId);
                });

            migrationBuilder.CreateTable(
                name: "TblArticulo",
                columns: table => new
                {
                    ArticuloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioOferta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProveedorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Destacado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CantidadVentas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblArticulo", x => x.ArticuloId);
                    table.ForeignKey(
                        name: "FK_TblArticulo_TblCategoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "TblCategoria",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TblArticulo_TblProveedor_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "TblProveedor",
                        principalColumn: "ProveedorId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblArticulo_CategoriaId",
                table: "TblArticulo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_TblArticulo_Codigo",
                table: "TblArticulo",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblArticulo_Nombre",
                table: "TblArticulo",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_TblArticulo_ProveedorId",
                table: "TblArticulo",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_TblCategoria_Nombre",
                table: "TblCategoria",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblProveedor_Nombre",
                table: "TblProveedor",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblArticulo");

            migrationBuilder.DropTable(
                name: "TblCategoria");

            migrationBuilder.DropTable(
                name: "TblProveedor");
        }
    }
}
