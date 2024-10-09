using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public decimal Balance { get; set; }
        public List<UserAsset> Assets { get; set; } 
        public List<Order> Orders { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ToroAccount ToroAccount { get; set; }

        // Verifica se o usuário tem saldo suficiente para realizar a compra
        public bool HasSufficientBalance(decimal totalAmount)
        {
            return this.Balance >= totalAmount;
        }

        // Debita o saldo após a compra
        public void DebitBalance(decimal amount)
        {
            if (HasSufficientBalance(amount))
            {
                this.Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Saldo insuficiente.");
            }
        }

        public bool IsValidCPF()
        {
            return CPF.Length == 11; // Simples verificação de comprimento
        }

        public bool CheckPassword(string password)
        {
            // Aqui, por simplicidade, comparei diretamente a senha.
            // Em produção, é para usar hashing adequado 
            return PasswordHash == password;
        }

    }

}
