using RecurringBitcoinPurchaseInstructions.Data;
using RecurringBitcoinPurchaseInstructions.Models;
using Microsoft.EntityFrameworkCore;
using RecurringBitcoinPurchaseInstructions.Interfaces;
using System.Data;
using System.Transactions;

namespace RecurringBitcoinPurchaseInstructions.Services
{
    /// <summary>
    /// Talimat işlemlerini yöneten bir servis sınıfıdır.
    /// </summary>
    public class InstructionService : IInstructionService
    {
        private readonly InstructionDbContext _dbContext;
        public InstructionService(InstructionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Yeni bir talimat ekler.
        /// </summary>
        /// <param name="request">Eklenen talimatın ayrıntılarını içeren talep nesnesi.</param>
        /// <returns>Eklenen talimatın bilgilerini içeren Instruction nesnesi.</returns>
        public async Task<Instruction> AddInstruction(CreateInstructionsRequest request)
        {
            // Kullanıcı talebini doğrular.
            await ValidationHelper.IsValidNewInstructionRequest(request);

            using (var scope = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = await GetUser(request.UserId);

                    var activeInstruction = await GetActiveInstruction(user);

                    // Yeni bir Bitcoin alım talimatı oluştur ve ekle
                    var createdInstruction = await CreateAndAddInstruction(request);

                    // Bildirimleri ekle
                    await AddNotifications(request.NotificationTypes, createdInstruction);

                    await scope.CommitAsync();

                    return createdInstruction;
                }
                catch (Exception e)
                {
                    await scope.RollbackAsync();
                    throw new TransactionAbortedException(e.ToString());
                }
            }
        }

        /// <summary>
        /// Kullanıcıyı ID'ye göre getirir.
        /// </summary>
        /// <param name="userId">Kullanıcının ID'si.</param>
        /// <returns>Belirtilen kullanıcının bilgilerini içeren User nesnesi.</returns>
        private async Task<User> GetUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }

            return user;
        }

        /// <summary>
        /// Belirtilen kullanıcının aktif talimatını getirir.
        /// </summary>
        /// <param name="user">Kullanıcı nesnesi.</param>
        /// <returns>Belirtilen kullanıcının aktif talimatını içeren Instruction nesnesi veya null (aktif talimat yoksa).</returns>
        private async Task<Instruction> GetActiveInstruction(User user)
        {
            var activeInstruction = await _dbContext.Instructions.FirstOrDefaultAsync(o => o.UserId == user.Id && o.IsActive);

            if (activeInstruction != null)
            {
                string errorMessage = $"User has an active instruction - Instruction ID: {activeInstruction.Id}";
                throw new ConstraintException(errorMessage);
            }

            return activeInstruction;
        }

        /// <summary>
        /// Yeni bir talimat oluşturur ve veritabanına ekler.
        /// </summary>
        /// <param name="request">Eklenen talimatın ayrıntılarını içeren talep nesnesi.</param>
        /// <returns>Eklenen talimatın bilgilerini içeren Instruction nesnesi.</returns>
        private async Task<Instruction> CreateAndAddInstruction(CreateInstructionsRequest request)
        {
            var createdInstruction = await _dbContext.Instructions.AddAsync(
                new Instruction(request.UserId, DateTime.Now, request.Amount, request.TransactionDay, true)
            );

            await _dbContext.SaveChangesAsync();

            return createdInstruction.Entity;
        }

        /// <summary>
        /// Belirtilen bildirim türlerini belirtilen talimata ekler.
        /// </summary>
        /// <param name="notificationTypes">Eklenecek bildirim türleri listesi.</param>
        /// <param name="instruction">Bildirimlerin ekleneceği talimat nesnesi.</param>
        private async Task AddNotifications(List<NotificationType> notificationTypes, Instruction instruction)
        {
            var distinctNotificationTypes = notificationTypes.Where(t => t != NotificationType.None).Distinct().ToList();

            foreach (var type in distinctNotificationTypes)
            {
                await _dbContext.Set<Notification>().AddAsync(new Notification(instruction.Id, type, true, DateTime.Now));
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip bir talimatı iptal eder.
        /// </summary>
        /// <param name="id">İptal edilecek talimatın ID'si.</param>
        public async Task CancelInstruction(int id)    
        {
            var instruction = await _dbContext.Instructions.FirstOrDefaultAsync(t => t.Id == id);

            if (instruction == null)
            {
                string errorMessage = "No instruction was found with the provided ID.";
                throw new ArgumentNullException(errorMessage);
            }
           
            var notifications = await _dbContext.Notifications.Where(t => t.InstructionId == id).ToListAsync();

            using (var scope = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    instruction.Deactivate();
                    _dbContext.Update(instruction);

                    notifications.ForEach(t => t.Deactivate());

                    _dbContext.UpdateRange(notifications);

                    await _dbContext.SaveChangesAsync();

                    await scope.CommitAsync();
                }
                catch (Exception e)
                {
                    await scope.RollbackAsync();
                    throw new TransactionAbortedException(e.ToString());
                }
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Belirtilen ID'ye sahip bir talimatı getirir.
        /// </summary>
        /// <param name="id">Getirilecek talimatın ID'si.</param>
        /// <returns>Belirtilen talimatın bilgilerini içeren Instruction nesnesi.</returns>
        public async Task<Instruction> GetInstructionById(int id)
        {
            var instruction = await _dbContext.Instructions.FirstOrDefaultAsync(t => t.Id == id);

            if (instruction == null)
            {
                string errorMessage = "No instruction was found. The specified instruction was not found.";
                throw new ArgumentNullException(errorMessage);
            }               

            return await Task.FromResult(instruction);
        }
        /// <summary>
        /// Belirli bir kullanıcıya ait pasif talimatları veya belirli bir miktarı filtreleyerek getirir.
        /// </summary>
        /// <param name="userId">Talimatları filtrelemek için kullanıcı ID'si (isteğe bağlı).</param>
        /// <param name="amount">Talimatları belirli bir miktarla filtrelemek için (isteğe bağlı).</param>
        /// <returns>Pasif talimatlar listesi.</returns>
        /// <exception cref="InvalidOperationException">Belirtilen kriterlere uygun talimat bulunamazsa.</exception>
        public async Task<List<Instruction>> GetInactiveInstructionsByUserAndAmount(int? userId, decimal? amount)
        {
            var query = _dbContext.Instructions.Where(t => !t.IsActive); // Yalnızca pasif talimatları alır.

            if (userId.HasValue)
            {
                query = query.Where(t => t.UserId == userId);
            }

            if (amount.HasValue)
            {
                query = query.Where(t => t.Amount == amount.Value);
            }


            var inactiveInstructions = await query.ToListAsync();

            if (inactiveInstructions.Count == 0)
            {
                string errorMessage = "No instructions found with the specified criteria.";
                throw new InvalidOperationException(errorMessage);
            }

            return inactiveInstructions;
        }
    }
}
