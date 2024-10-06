﻿using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
       Task<List<Order>> GetOrdersByUserId(int userId);
    }

}
