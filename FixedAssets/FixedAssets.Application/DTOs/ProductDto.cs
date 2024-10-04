using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Indexer { get; set; }
        public decimal Tax { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
}
