using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{
    // Representa um ativo adquirido por um usuário
    public class UserAsset
    {
        public int ProductId { get; set; } // ID do produto (ativo)
        public string ProductName { get; set; } // Nome do produto adquirido
        public int Quantity { get; set; } // Quantidade do ativo adquirido
    }
}
