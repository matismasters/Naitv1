using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class agregarListaParticipantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActividadId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MensajeDelAnfitrion",
                table: "Actividades",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ActividadId",
                table: "Usuarios",
                column: "ActividadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Actividades_ActividadId",
                table: "Usuarios",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Actividades_ActividadId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ActividadId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ActividadId",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "MensajeDelAnfitrion",
                table: "Actividades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
