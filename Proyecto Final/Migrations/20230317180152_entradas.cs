using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class entradas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "entradas",
                newName: "existencias_iniciales");

            migrationBuilder.AddColumn<int>(
                name: "existencias",
                table: "entradas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "existencias",
                table: "entradas");

            migrationBuilder.RenameColumn(
                name: "existencias_iniciales",
                table: "entradas",
                newName: "cantidad");
        }
    }
}
