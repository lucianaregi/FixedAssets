using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);     
    }
}
