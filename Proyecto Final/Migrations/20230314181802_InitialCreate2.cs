using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedido_tiene_productos_Pedidos_id_producto",
                table: "pedido_tiene_productos");

            migrationBuilder.DropIndex(
                name: "IX_pedido_tiene_productos_id_producto",
                table: "pedido_tiene_productos");

            migrationBuilder.AddColumn<int>(
                name: "id_pedido",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_producto",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pedidoid",
                table: "pedido_tiene_productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_id_producto",
                table: "Productos",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_tiene_productos_pedidoid",
                table: "pedido_tiene_productos",
                column: "pedidoid");

            migrationBuilder.AddForeignKey(
                name: "FK_pedido_tiene_productos_Pedidos_pedidoid",
                table: "pedido_tiene_productos",
                column: "pedidoid",
                principalTable: "Pedidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Pedidos_id_producto",
                table: "Productos",
                column: "id_producto",
                principalTable: "Pedidos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pedido_tiene_productos_Pedidos_pedidoid",
                table: "pedido_tiene_productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Pedidos_id_producto",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_id_producto",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_pedido_tiene_productos_pedidoid",
                table: "pedido_tiene_productos");

            migrationBuilder.DropColumn(
                name: "id_pedido",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "id_producto",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "pedidoid",
                table: "pedido_tiene_productos");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_tiene_productos_id_producto",
                table: "pedido_tiene_productos",
                column: "id_producto");

            migrationBuilder.AddForeignKey(
                name: "FK_pedido_tiene_productos_Pedidos_id_producto",
                table: "pedido_tiene_productos",
                column: "id_producto",
                principalTable: "Pedidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
