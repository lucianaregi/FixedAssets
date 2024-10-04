using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id); // Retorna um usuário específico por ID
        Task UpdateUserAsync(User user);     // Atualiza o saldo do usuário após a compra
    }
}
