﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{


    public class UserAsset
    {
        public int UserId { get; set; } 
        public int ProductId { get; set; } 

        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public User User { get; set; } 
        public Product Product { get; set; } 
    }


}
