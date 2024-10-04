using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } // Identificador único do produto
        public string Name { get; set; } // Nome do produto (ex: CDB, LCI, etc)
        public string Indexer { get; set; } // Indexador (IPCA, Selic, etc)
        public decimal Tax { get; set; } // Taxa de retorno do produto
        public decimal UnitPrice { get; set; } // Preço unitário do produto
        public int Stock { get; set; } // Estoque disponível para venda

        // Validação se o produto tem estoque suficiente
        public bool HasSufficientStock(int quantity)
        {
            return this.Stock >= quantity;
        }

        // Atualiza o estoque após uma compra
        public void DebitStock(int quantity)
        {
            if (HasSufficientStock(quantity))
            {
                this.Stock -= quantity;
            }
            else
            {
                throw new InvalidOperationException("Estoque insuficiente.");
            }
        }
    }
}
