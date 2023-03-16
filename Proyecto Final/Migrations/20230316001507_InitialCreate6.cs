using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "precio",
                table: "Productos",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "producto_tiene_componentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idproducto = table.Column<int>(name: "id_producto", type: "int", nullable: false),
                    idcomponente = table.Column<int>(name: "id_componente", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto_tiene_componentes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "producto_tiene_componentes");

            migrationBuilder.DropColumn(
                name: "precio",
                table: "Productos");
        }
    }
}
