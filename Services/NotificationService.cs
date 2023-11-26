using RecurringBitcoinPurchaseInstructions.Data;
using RecurringBitcoinPurchaseInstructions.Models;
using Microsoft.EntityFrameworkCore;
using RecurringBitcoinPurchaseInstructions.Interfaces;
using System.Net.Http;

namespace RecurringBitcoinPurchaseInstructions.Services
{
    /// <summary>
    /// Bildirim işlemleriyle ilgili hizmetleri sağlayan bir hizmet sınıfı.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly InstructionDbContext _dbContext;
        private Uri BaseApiUri { get; set; }
        public NotificationService(ILogger<NotificationService> logger, InstructionDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            BaseApiUri = new Uri(configuration["NotificationServiceBaseUrl"] ?? "http://localhost:5001/api/v1/");
        }
        /// <summary>
        /// Yeni bir bildirim ekler.
        /// </summary>
        /// <param name="request">Eklenen bildirimin ayrıntılarını içeren talep nesnesi.</param>
        public async Task AddNotification(CreateNotificationRequest request)
        {
            await ValidationHelper.IsValidAddNotificationRequest(request);

            var instruction = await _dbContext.Instructions.FirstOrDefaultAsync(t => t.Id == request.InstructionId && t.IsActive);

            if (instruction == null)
            {
                string errorMessage = "No active instruction found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }

            var existedNotifications = await _dbContext.Notifications.Where(t => t.InstructionId == request.InstructionId && request.NotificationTypes.Contains(t.NotificationType)).ToListAsync();

            if (existedNotifications.Any())
            {
                existedNotifications.ForEach(t => t.Activate());

                _dbContext.Notifications.UpdateRange(existedNotifications);
            }

            var notExistedNotifications = request.NotificationTypes.Except(existedNotifications.Select(t => t.NotificationType).ToList()).ToList();

            notExistedNotifications.ForEach(async t =>
            {
                await _dbContext.Notifications.AddAsync(new Notification(request.InstructionId, t, true, DateTime.Now));
            });

            await _dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Belirtilen talimatın ID'sine göre bildirimleri getirir.
        /// </summary>
        /// <param name="instructionId">Talimatın ID'si.</param>
        /// <returns>Talimat için bulunan bildirimlerin listesi.</returns>
        public async Task<List<Notification>> GetNotificaitonByInstructionId(int instructionId)
        {
            var instruction = await _dbContext.Instructions.FirstOrDefaultAsync(t => t.Id == instructionId);

            if (instruction == null)
            {
                string errorMessage = "No instruction was found. The specified instruction was not found.";
                throw new ArgumentNullException(errorMessage);
            }
            var notifications = await _dbContext.Notifications.Where(t => t.InstructionId == instructionId).ToListAsync();

            if (!notifications.Any())
            {
                string errorMessage = "No notifications were found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }


            return await Task.FromResult(notifications);
        }

        /// <summary>
        /// Belirtilen kullanıcının bildirimlerini getirir.
        /// </summary>
        /// <param name="userId">Kullanıcının ID'si.</param>
        /// <param name="isActive">Etkin bildirimleri mi getirilsin?</param>
        /// <returns>Kullanıcı için bulunan bildirimlerin listesi.</returns>
        public async Task<List<Notification>> GetByUserId(int userId, bool isActive = true)
        {
            var instruction = await _dbContext.Instructions.Where(t => t.UserId == userId).ToListAsync();

            if (instruction == null)
            {
                string errorMessage = "No instruction was found. The specified instruction was not found.";
                throw new ArgumentNullException(errorMessage);
            }

            var notifications = await _dbContext.Notifications.Where(t => instruction.Select(o => o.Id).ToList().Contains(t.InstructionId)).ToListAsync();

            if (!notifications.Any())
            {
                string errorMessage = "No notifications were found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }


            return await Task.FromResult(notifications);
        }

        /// <summary>
        /// Belirtilen talimatın ID'sine göre bildirim kayıtlarını getirir.
        /// </summary>
        /// <param name="instructionId">Talimatın ID'si.</param>
        /// <returns>Talimat için bulunan bildirim kayıtlarının listesi.</returns>
        public async Task<List<NotificationLog>> GetNotificationLogsByInstructionId(int instructionId)
        {
            var instruction = await _dbContext.Instructions.FirstOrDefaultAsync(t => t.Id == instructionId);

            if (instruction == null)
            {
                string errorMessage = "No instruction was found. The specified instruction was not found.";
                throw new ArgumentNullException(errorMessage);
            }

            var notifications = await _dbContext.Notifications.Where(t => t.InstructionId == instructionId).ToListAsync();

            if (!notifications.Any())
            {
                string errorMessage = "No notifications were found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }


            var notificationLogs = await _dbContext.NotificationLogs.Where(t => notifications.Select(n => n.Id).ToList().Contains(t.NotificationId)).ToListAsync();

            if (!notificationLogs.Any())
            {
                string errorMessage = "No notification logs found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }


            return await Task.FromResult(notificationLogs);
        }

        /// <summary>
        /// Belirtilen kullanıcının bildirim kayıtlarını getirir.
        /// </summary>
        /// <param name="userId">Kullanıcının ID'si.</param>
        /// <returns>Kullanıcı için bulunan bildirim kayıtlarının listesi.</returns>
        public async Task<List<NotificationLog>> GetNotificationLogsByUserId(int userId)
        {
            var instruction = await _dbContext.Instructions.Where(t => t.UserId == userId).ToListAsync();

            if (!instruction.Any())
            {
                string errorMessage = "No instruction was found. The specified instruction was not found.";
                throw new ArgumentNullException(errorMessage);
            }

            
            var notifications = await _dbContext.Notifications.Where(t => instruction.Select(o => o.Id).ToList().Contains(t.InstructionId)).ToListAsync();

            if (!notifications.Any())
            {
                string errorMessage = "No notifications were found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }

            var notificationLogs = await _dbContext.NotificationLogs.Where(t => notifications.Select(n => n.Id).ToList().Contains(t.NotificationId)).ToListAsync();

            if (!notificationLogs.Any())
            {
                string errorMessage = "No notification logs found for the current user.";
                throw new ArgumentNullException(errorMessage);
            }

            return await Task.FromResult(notificationLogs);
        }

        /// <summary>
        /// Belirtilen kullanıcı için bildirim gönderir.
        /// </summary>
        /// <param name="userId">Kullanıcının ID'si.</param>
     
        public async Task SendNotificationByUserId(int userId)
        {
            int id = 0; 
            try
            {
                var instruction = await _dbContext.Instructions.FirstOrDefaultAsync(t => t.UserId == userId && t.IsActive);

                if (instruction == null)
                {
                    throw new InvalidOperationException("No active instruction was found for the current user.");
                }

                var notifications = await _dbContext.Notifications.Where(t => t.InstructionId == instruction.Id && t.IsActive).ToListAsync();

                if (!notifications.Any())
                {
                    throw new InvalidOperationException("No active notifications were found for the current user.");
                }
                id = instruction.Id;
                foreach (var notification in notifications)
                {
                    var notificationText = NotificationHelper.GetNotificationText(notification.NotificationType, instruction);
                    _logger.LogInformation("Notification sent successfully.");

                    var successLog = new NotificationLog(
                        notification.Id,
                        $"Talimat Id'si {instruction.Id} olan talimat için {notificationText.First().Key} ile `{notificationText.First().Value}` bildirimi yapıldı.",
                        DateTime.Now);

                    await _dbContext.NotificationLogs.AddAsync(successLog);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var errorLog = new NotificationLog (id, "Error in SendNotificationByUserId: " + ex.Message,
                       DateTime.Now);
                await _dbContext.NotificationLogs.AddAsync(errorLog);
                await _dbContext.SaveChangesAsync();
              
            }
        }

    }
}