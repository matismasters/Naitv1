using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class seAgregaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActividadesUsuarios_Actividades_ActividadId",
                table: "ActividadesUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_ActividadesUsuarios_Usuarios_UsuarioId",
                table: "ActividadesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_ActividadesUsuarios_ActividadId",
                table: "ActividadesUsuarios");

            migrationBuilder.DropIndex(
                name: "IX_ActividadesUsuarios_UsuarioId",
                table: "ActividadesUsuarios");

            migrationBuilder.DropColumn(
                name: "ActividadId",
                table: "ActividadesUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "ActividadesUsuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActividadId",
                table: "ActividadesUsuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "ActividadesUsuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesUsuarios_ActividadId",
                table: "ActividadesUsuarios",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesUsuarios_UsuarioId",
                table: "ActividadesUsuarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActividadesUsuarios_Actividades_ActividadId",
                table: "ActividadesUsuarios",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActividadesUsuarios_Usuarios_UsuarioId",
                table: "ActividadesUsuarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
