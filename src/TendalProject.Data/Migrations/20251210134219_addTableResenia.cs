using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TendalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class addTableResenia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblReseña",
                columns: table => new
                {
                    ReseñaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArticuloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Puntuacion = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblReseña", x => x.ReseñaId);
                    table.ForeignKey(
                        name: "FK_TblReseña_TblArticulo_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "TblArticulo",
                        principalColumn: "ArticuloId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblReseña_TblCliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "TblCliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblReseña_ArticuloId",
                table: "TblReseña",
                column: "ArticuloId");

            migrationBuilder.CreateIndex(
                name: "IX_TblReseña_ClienteId",
                table: "TblReseña",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblReseña");
        }
    }
}
