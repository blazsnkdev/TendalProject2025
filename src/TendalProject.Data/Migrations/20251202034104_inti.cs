using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TendalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class inti : Migration
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
                name: "TblRol",
                columns: table => new
                {
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblRol", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "TblUsuario",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UltimaConexion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsuario", x => x.UsuarioId);
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
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProveedorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "TblCliente",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCelular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCliente", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_TblCliente_TblUsuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TblUsuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUsuarioRol",
                columns: table => new
                {
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUsuarioRol", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_TblUsuarioRol_TblRol_RolId",
                        column: x => x.RolId,
                        principalTable: "TblRol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblUsuarioRol_TblUsuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TblUsuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_TblCliente_UsuarioId",
                table: "TblCliente",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblProveedor_Nombre",
                table: "TblProveedor",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblUsuarioRol_RolId",
                table: "TblUsuarioRol",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblArticulo");

            migrationBuilder.DropTable(
                name: "TblCliente");

            migrationBuilder.DropTable(
                name: "TblUsuarioRol");

            migrationBuilder.DropTable(
                name: "TblCategoria");

            migrationBuilder.DropTable(
                name: "TblProveedor");

            migrationBuilder.DropTable(
                name: "TblRol");

            migrationBuilder.DropTable(
                name: "TblUsuario");
        }
    }
}
