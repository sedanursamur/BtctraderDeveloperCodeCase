using RecurringBitcoinPurchaseInstructions.Data;
using RecurringBitcoinPurchaseInstructions.Models;

namespace RecurringBitcoinPurchaseInstructions.Interfaces
{
    public interface IInstructionService
    {
        Task<Instruction> AddInstruction(CreateInstructionsRequest request);
        Task<Instruction> GetInstructionById(int id);
        Task<List<Instruction>> GetInactiveInstructionsByUserAndAmount(int? userId, decimal? amount);
        Task CancelInstruction(int id);
      
    }
}
