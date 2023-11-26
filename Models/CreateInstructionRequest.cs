using RecurringBitcoinPurchaseInstructions.Data;

namespace RecurringBitcoinPurchaseInstructions.Models
{

    public class CreateInstructionsRequest
    {
        public int UserId { get; set; }
        public int TransactionDay { get; set; }
        public List<NotificationType> NotificationTypes { get; set; }
        public decimal Amount { get; set; }

        public CreateInstructionsRequest()
        {
            NotificationTypes = new();
        }

    }
}
