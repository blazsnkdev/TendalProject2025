using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TendalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class addColumnsUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadLogins",
                table: "TblUsuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDesbloqueo",
                table: "TblUsuario",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadLogins",
                table: "TblUsuario");

            migrationBuilder.DropColumn(
                name: "FechaDesbloqueo",
                table: "TblUsuario");
        }
    }
}
