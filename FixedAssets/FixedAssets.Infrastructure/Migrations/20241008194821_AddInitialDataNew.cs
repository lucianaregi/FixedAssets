using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialDataNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir usuários
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "CPF", "Balance", "Email", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "Julia Barbosa", "12345678901", 0m,"julia.barbosa@example.com","1234" },
                    { 2, "Pedro Silva", "23456789012", 0m,"pedro.silva@example.com","1234" },
                    { 3, "Mariana Costa", "34567890123", 0m,"mariana.costa@example.com","1234" }
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
                    { 3, "Produto 3", "Selic", 6.0m, 300m, 70 },
                    { 4, "LCA", "CDI", 5.8m, 1500m, 80 },
                    { 5, "Debênture", "IPCA", 7.5m, 2500m, 40 },
                    { 6, "Tesouro Direto", "Selic", 4.5m, 1200m, 150 },
                    { 7, "Fundo Imobiliário", "IFIX", 8.0m, 3000m, 30 },
                    { 8, "Letra de Câmbio", "CDI", 6.2m, 1800m, 60 },
                    { 9, "CRI", "IPCA", 7.8m, 2200m, 45 },
                    { 10, "Fundo de Ações", "Ibovespa", 9.0m, 2800m, 55 }
                }
            );

            // Inserir a conta
            migrationBuilder.InsertData(
                table: "ToroAccounts",
                columns: new[] { "UserId", "AccountNumber", "Balance" },
                values: new object[,]
                {
                    { 1, "00001", 15000m },
                    { 2, "00002", 20000m },
                    { 3, "00003", 12000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ToroAccounts");

            migrationBuilder.DropTable(
                name: "UserAssets");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
