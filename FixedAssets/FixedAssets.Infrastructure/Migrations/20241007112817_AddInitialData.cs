using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FixedAssets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir usuários
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "CPF", "Balance" },
                values: new object[,]
                {
                    { 1, "Luciana Rocha", "12345678901", 1500m },
                    { 2, "Pedro Silva", "23456789012", 2000m },
                    { 3, "Mariana Costa", "34567890123", 1200m }
                }
            );

            // Inserir produtos
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Indexer", "Tax", "UnitPrice", "Stock" },
                values: new object[,]
                {
                    { 1, "Produto 1", "IPCA", 5.5m, 100m, 100 },
                    { 2, "Produto 2", "CDI", 7.2m, 200m, 50 },
                    { 3, "Produto 3", "Selic", 6.0m, 300m, 70 }
                }
            );

            // Inserir ordens
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "UserId", "OrderDate" },
                values: new object[,]
                {
                    { 1, 1, DateTime.UtcNow },
                    { 2, 2, DateTime.UtcNow },
                    { 3, 3, DateTime.UtcNow }
                }
            );

            // Inserir ativos do usuário
            migrationBuilder.InsertData(
                table: "UserAssets",
                columns: new[] { "UserId", "ProductId", "ProductName", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "Produto 1", 10 },
                    { 2, 2, "Produto 2", 5 },
                    { 3, 3, "Produto 3", 7 }
                }
            );

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
            // Remover os dados inseridos na migração 
            migrationBuilder.DeleteData(table: "OrderItems", keyColumns: new[] { "OrderId", "ProductId" }, keyValues: new object[] { 1, 1 });
            migrationBuilder.DeleteData(table: "OrderItems", keyColumns: new[] { "OrderId", "ProductId" }, keyValues: new object[] { 2, 2 });
            migrationBuilder.DeleteData(table: "OrderItems", keyColumns: new[] { "OrderId", "ProductId" }, keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(table: "UserAssets", keyColumns: new[] { "UserId", "ProductId" }, keyValues: new object[] { 1, 1 });
            migrationBuilder.DeleteData(table: "UserAssets", keyColumns: new[] { "UserId", "ProductId" }, keyValues: new object[] { 2, 2 });
            migrationBuilder.DeleteData(table: "UserAssets", keyColumns: new[] { "UserId", "ProductId" }, keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValue: 3);

            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: 3);

            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Users", keyColumn: "Id", keyValue: 3);
        }
    }
}
