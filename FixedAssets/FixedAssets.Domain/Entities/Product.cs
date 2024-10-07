using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Indexer { get; set; }
        public decimal Tax { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<UserAsset> UserAssets { get; set; }

       
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

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new InvalidOperationException("O preço do produto não pode ser negativo.");
            }
            this.UnitPrice = price;
        }

    }

}
