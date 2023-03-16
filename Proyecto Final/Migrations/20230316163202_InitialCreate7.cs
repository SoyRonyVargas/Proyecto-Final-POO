using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Productos_Productoid",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Productoid",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Productoid",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "Productoid",
                table: "Componentes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Componentes_Productoid",
                table: "Componentes",
                column: "Productoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Componentes_Productos_Productoid",
                table: "Componentes",
                column: "Productoid",
                principalTable: "Productos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Componentes_Productos_Productoid",
                table: "Componentes");

            migrationBuilder.DropIndex(
                name: "IX_Componentes_Productoid",
                table: "Componentes");

            migrationBuilder.DropColumn(
                name: "Productoid",
                table: "Componentes");

            migrationBuilder.AddColumn<int>(
                name: "Productoid",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Productoid",
                table: "Productos",
                column: "Productoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Productos_Productoid",
                table: "Productos",
                column: "Productoid",
                principalTable: "Productos",
                principalColumn: "id");
        }
    }
}
