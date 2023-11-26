
namespace RecurringBitcoinPurchaseInstructions.Data
{
    public class Instruction
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public decimal Amount { get; private set; }
        public int TransactionDay { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? ModifiedOn { get; private set; }

        public Instruction(int userId, DateTime createdOn, decimal amount, int transactionDay, bool isActive, DateTime? modifiedOn = null)
        {
            UserId = userId;
            CreatedOn = createdOn;
            Amount = amount;
            TransactionDay = transactionDay;
            IsActive = isActive;
            ModifiedOn = modifiedOn;
        }

        public void Activate()
        {
            IsActive = true;
            ModifiedOn = DateTime.Now;
        }

        public void Deactivate()
        {
            IsActive = false;
            ModifiedOn = DateTime.Now;
        }
    }
}
