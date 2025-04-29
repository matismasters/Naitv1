using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naitv1.Migrations
{
    /// <inheritdoc />
    public partial class actualizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoPartner",
                table: "Partners");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Partners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Telefono",
                table: "Partners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Partners");

            migrationBuilder.AddColumn<bool>(
                name: "EstadoPartner",
                table: "Partners",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
