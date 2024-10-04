using System.Text.Json;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;

namespace FixedAssets.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _filePath = "data/users.json"; // Caminho do arquivo JSON de usuários

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (!File.Exists(_filePath)) return null;

            var jsonData = await File.ReadAllTextAsync(_filePath);
            var users = JsonSerializer.Deserialize<List<User>>(jsonData);
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            var users = await GetAllUsersAsync();
            var userToUpdate = users.FirstOrDefault(u => u.Id == user.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Balance = user.Balance;
                userToUpdate.Assets = user.Assets;
                var jsonData = JsonSerializer.Serialize(users);
                await File.WriteAllTextAsync(_filePath, jsonData);
            }
        }

        private async Task<List<User>> GetAllUsersAsync()
        {
            if (!File.Exists(_filePath)) return new List<User>();

            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<User>>(jsonData);
        }
    }
}
