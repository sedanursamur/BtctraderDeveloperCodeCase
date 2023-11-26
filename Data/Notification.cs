using RecurringBitcoinPurchaseInstructions.Models;

namespace RecurringBitcoinPurchaseInstructions.Data
{
    public class Notification
    {
        public int Id { get;  private set; }
        public int InstructionId { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? ModifiedOn { get; private set; }

        public Notification(int instructionId, NotificationType notificationType, bool isActive, DateTime createdOn, DateTime? modifiedOn = null)
        {
            InstructionId = instructionId;
            NotificationType = notificationType;
            IsActive = isActive;
            CreatedOn = createdOn;
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