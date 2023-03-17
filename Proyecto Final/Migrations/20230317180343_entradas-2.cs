using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class entradas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_entradas",
                table: "entradas");

            migrationBuilder.RenameTable(
                name: "entradas",
                newName: "Entradas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entradas",
                table: "Entradas",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Entradas",
                table: "Entradas");

            migrationBuilder.RenameTable(
                name: "Entradas",
                newName: "entradas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_entradas",
                table: "entradas",
                column: "id");
        }
    }
}
