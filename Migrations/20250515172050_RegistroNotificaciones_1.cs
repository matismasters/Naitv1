using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class RegistroNotificaciones_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_Usuarios_AnfitrionId",
                table: "Actividades");

            migrationBuilder.CreateTable(
                name: "RegistroNotificaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoNotificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNotificacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroNotificaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroNotificaciones_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RegistroNotificaciones_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroNotificaciones_ActividadId",
                table: "RegistroNotificaciones",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroNotificaciones_UsuarioId",
                table: "RegistroNotificaciones",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividades_Usuarios_AnfitrionId",
                table: "Actividades",
                column: "AnfitrionId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_Usuarios_AnfitrionId",
                table: "Actividades");

            migrationBuilder.DropTable(
                name: "RegistroNotificaciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividades_Usuarios_AnfitrionId",
                table: "Actividades",
                column: "AnfitrionId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
