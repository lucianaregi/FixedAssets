using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Domain.Entities
{
    public class User
    {
        public int Id { get; set; } // Identificador único do usuário
        public string Name { get; set; } // Nome do usuário
        public string CPF { get; set; } // CPF do usuário, para efeito de identificação
        public decimal Balance { get; set; } // Saldo disponível para compras
        public List<UserAsset> Assets { get; set; } // Lista de ativos adquiridos pelo usuário

        // Validação se o usuário tem saldo suficiente para realizar a compra
        public bool HasSufficientBalance(decimal totalAmount)
        {
            return this.Balance >= totalAmount;
        }

        // Atualiza o saldo do usuário após uma compra
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
    }
}
