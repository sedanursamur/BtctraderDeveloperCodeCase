using RecurringBitcoinPurchaseInstructions.Data;
using System.Text;

namespace RecurringBitcoinPurchaseInstructions.Models
{
    public enum NotificationType
    {
        None = 0,
        SMS = 1,
        Email = 2,
        PushNotification = 3
    }
    public static class NotificationHelper
    {
        public static Dictionary<string, string> GetNotificationText(NotificationType t, Instruction instruction)
        {
            Dictionary<string, string> response = new();
            var notificationText = $"Değerli müşterimiz, her ayın {instruction.TransactionDay}. günü için oluşturmuş olduğunuz {instruction.Amount} ₺ tutarlı talimatınız gerçekleştirilmiştir.";
            switch (t)
            {
                case NotificationType.SMS:
                    response.Add("SMS", notificationText);
                    break;
                case NotificationType.Email:
                    response.Add("Email", notificationText);
                    break;
                case NotificationType.PushNotification:
                    response.Add("PushNotification", notificationText);
                    break;
            }

            return response;
        }
    }
}
