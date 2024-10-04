using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.DTOs
{
    public class OrderDto
    {
        public int UserId { get; set; } // ID do usuário que está realizando a compra
        public int ProductId { get; set; } // ID do produto a ser comprado
        public int Quantity { get; set; } // Quantidade a ser comprada
    }
}
