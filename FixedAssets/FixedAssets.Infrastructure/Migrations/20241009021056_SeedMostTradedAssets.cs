using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedAssets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMostTradedAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona 5 ativos à tabela MostTradedAssets
            migrationBuilder.InsertData(
            table: "MostTradedAssets",
            columns: new[] { "Id", "Name", "CurrentValue", "TotalTrades" },
            values: new object[,]
            {
                { 1, "Ativo A", 100.50m, 50 },
                { 2, "Ativo B", 200.75m, 70 },
                { 3, "Ativo C", 150.00m, 40 },
                { 4, "Ativo D", 250.30m, 100 },
                { 5, "Ativo E", 300.10m, 90 }
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove os ativos adicionados
            migrationBuilder.DeleteData(
                table: "MostTradedAssets",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5 });
        }
    }
}
