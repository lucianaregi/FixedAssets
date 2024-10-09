using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{
    public class MostTradedAsset
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal CurrentValue { get; set; } 
        public int TotalTrades { get; set; } 
    }

}
