using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "ActividadesUsuarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ActividadId",
                table: "ActividadesUsuarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActividadesUsuarios_Actividades_ActividadId",
                table: "ActividadesUsuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_ActividadesUsuarios_Usuarios_UsuarioId",
                table: "ActividadesUsuarios");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "ActividadesUsuarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActividadId",
                table: "ActividadesUsuarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActividadesUsuarios_Actividades_ActividadId",
                table: "ActividadesUsuarios",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActividadesUsuarios_Usuarios_UsuarioId",
                table: "ActividadesUsuarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
