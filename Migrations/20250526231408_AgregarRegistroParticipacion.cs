using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRegistroParticipacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosParticipacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActividadId = table.Column<int>(type: "int", nullable: false),
                    ParticipanteId = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosParticipacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosParticipacion_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegistrosParticipacion_Usuarios_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParticipacion_ActividadId",
                table: "RegistrosParticipacion",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosParticipacion_ParticipanteId",
                table: "RegistrosParticipacion",
                column: "ParticipanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosParticipacion");
        }
    }
}
