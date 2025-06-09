using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoModeloCiudad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CiudadId",
                table: "Actividades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ciudad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudad", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_CiudadId",
                table: "Actividades",
                column: "CiudadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actividades_Ciudad_CiudadId",
                table: "Actividades",
                column: "CiudadId",
                principalTable: "Ciudad",
                principalColumn: "Id");
                /*onDelete: ReferentialAction.Cascade);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actividades_Ciudad_CiudadId",
                table: "Actividades");

            migrationBuilder.DropTable(
                name: "Ciudad");

            migrationBuilder.DropIndex(
                name: "IX_Actividades_CiudadId",
                table: "Actividades");

            migrationBuilder.DropColumn(
                name: "CiudadId",
                table: "Actividades");
        }
    }
}
