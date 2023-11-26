using System.ComponentModel.DataAnnotations;

namespace RecurringBitcoinPurchaseInstructions.Models
{
    public static class ValidationHelper
    {
        public static async Task IsValidNewInstructionRequest(CreateInstructionsRequest request)
        {
            request.NotificationTypes = request.NotificationTypes.Where(t => t != NotificationType.None).Distinct().ToList();
            if (request.UserId <= 0)
                throw new ValidationException("UserId should be a positive integer.");

            if (request.TransactionDay < 1 || request.TransactionDay > 28)
                throw new ValidationException("TransactionDay should be between 1 and 28.");

            if (request.Amount < 100 || request.Amount > 20000)
                throw new ValidationException("Amount should be between 100 and 20.000.");

            if (request.NotificationTypes.Count == 0)
                throw new ValidationException("At least one valid NotificationType is required.");

            await Task.CompletedTask;
        }
        public static async Task IsValidAddNotificationRequest(CreateNotificationRequest request)
        {
            request.NotificationTypes = request.NotificationTypes.Where(t => t != NotificationType.None).Distinct().ToList();

            if (request.InstructionId <= 0)
                throw new ValidationException("InstructionId should be a positive integer.");

            if(request.NotificationTypes.Count == 0)
                throw new ValidationException("At least one valid NotificationType is required.");

            await Task.CompletedTask;
        }
    }
}
