using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Pedidos_Pedidoid",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Pedidoid",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Pedidoid",
                table: "Productos");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_id_pedido",
                table: "Productos",
                column: "id_pedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Pedidos_id_pedido",
                table: "Productos",
                column: "id_pedido",
                principalTable: "Pedidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Pedidos_id_pedido",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_id_pedido",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "Pedidoid",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Pedidoid",
                table: "Productos",
                column: "Pedidoid");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Pedidos_Pedidoid",
                table: "Productos",
                column: "Pedidoid",
                principalTable: "Pedidos",
                principalColumn: "id");
        }
    }
}
