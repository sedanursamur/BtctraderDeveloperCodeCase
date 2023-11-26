namespace RecurringBitcoinPurchaseInstructions.Models
{
    public class CreateNotificationRequest
    {
        public int InstructionId { get; set; }
        public List<NotificationType> NotificationTypes { get; set; }

        public CreateNotificationRequest()
        {
            NotificationTypes = new();
        }
    }

}
