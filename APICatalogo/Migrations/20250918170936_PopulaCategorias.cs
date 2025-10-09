using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImageUrl) values ('Bebidas', 'bebidas.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImageUrl) values ('Lanches', 'lanches.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImageUrl) values ('Sobremesas', 'sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
