namespace RecurringBitcoinPurchaseInstructions.Data
{
    public class NotificationLog
    {
        public int Id { get; private set; }
        public int NotificationId { get; private set; }
        public string NotificationText { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public NotificationLog(int notificationId, string notificationText, DateTime createdOn)
        {
            NotificationId = notificationId;
            NotificationText = notificationText;
            CreatedOn = createdOn;
        }
    }
}
