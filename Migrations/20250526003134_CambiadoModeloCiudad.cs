using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class CambiadoModeloCiudad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_Ciudad_CiudadId",
                table: "Actividades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ciudad",
                table: "Ciudad");

            migrationBuilder.RenameTable(
                name: "Ciudad",
                newName: "Ciudades");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Ciudades",
                newName: "Nombre");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ciudades",
                table: "Ciudades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividades_Ciudades_CiudadId",
                table: "Actividades",
                column: "CiudadId",
                principalTable: "Ciudades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_Ciudades_CiudadId",
                table: "Actividades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ciudades",
                table: "Ciudades");

            migrationBuilder.RenameTable(
                name: "Ciudades",
                newName: "Ciudad");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Ciudad",
                newName: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ciudad",
                table: "Ciudad",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividades_Ciudad_CiudadId",
                table: "Actividades",
                column: "CiudadId",
                principalTable: "Ciudad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
