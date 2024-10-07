using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedOrderItemsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir itens de ordem
            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 2, 100m },  // Ordem 1 (Luciana Rocha) comprou 2 unidades do Produto 1
                    { 2, 2, 1, 200m },  // Ordem 2 (Pedro Silva) comprou 1 unidade do Produto 2
                    { 3, 3, 3, 300m }   // Ordem 3 (Mariana Costa) comprou 3 unidades do Produto 3
                }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
