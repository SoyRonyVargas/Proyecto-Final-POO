using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Pedidos_id_producto",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "id_producto",
                table: "Productos",
                newName: "Pedidoid");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_id_producto",
                table: "Productos",
                newName: "IX_Productos_Pedidoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Pedidos_Pedidoid",
                table: "Productos",
                column: "Pedidoid",
                principalTable: "Pedidos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Pedidos_Pedidoid",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "Pedidoid",
                table: "Productos",
                newName: "id_producto");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_Pedidoid",
                table: "Productos",
                newName: "IX_Productos_id_producto");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Pedidos_id_producto",
                table: "Productos",
                column: "id_producto",
                principalTable: "Pedidos",
                principalColumn: "id");
        }
    }
}
