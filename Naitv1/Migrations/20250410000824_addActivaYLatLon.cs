using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class addActivaYLatLon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lon",
                table: "Actividades",
                newName: "Lon");

            migrationBuilder.RenameColumn(
                name: "lat",
                table: "Actividades",
                newName: "Lat");

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Actividades",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Actividades");

            migrationBuilder.RenameColumn(
                name: "Lon",
                table: "Actividades",
                newName: "lon");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "Actividades",
                newName: "lat");
        }
    }
}
