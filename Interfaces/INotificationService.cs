using RecurringBitcoinPurchaseInstructions.Data;
using RecurringBitcoinPurchaseInstructions.Models;

namespace RecurringBitcoinPurchaseInstructions.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificaitonByInstructionId(int instructionId);
        Task<List<Notification>> GetByUserId(int userId, bool isActive = true);
        Task<List<NotificationLog>> GetNotificationLogsByInstructionId(int instructionId);
        Task<List<NotificationLog>> GetNotificationLogsByUserId(int userId);
        Task AddNotification(CreateNotificationRequest notification);
        Task SendNotificationByUserId(int userId);
    }
}
