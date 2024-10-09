namespace FixedAssets.Domain.Entities
{
    public class ToroAccount
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public string AccountNumber { get; set; } 
        public decimal Balance { get; set; } 

        // Relacionamento com o usuário
        public User User { get; set; }

        // Métodos para verificar saldo e debitar conta
        public bool HasSufficientBalance(decimal amount)
        {
            return Balance >= amount;
        }

        public void DebitBalance(decimal amount)
        {
            if (HasSufficientBalance(amount))
            {
                Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Saldo insuficiente.");
            }
        }
    }
}
