using RecurringBitcoinPurchaseInstructions.Models;
using Microsoft.AspNetCore.Mvc;
using RecurringBitcoinPurchaseInstructions.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RecurringBitcoinPurchaseInstructions.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;
        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Yeni bir bildirim ekler.
        /// </summary>
        /// <param name="request">CreateNotificationRequest �rne�i</param>
        /// <returns>HTTP 201 Created veya uygun bir hata durumu</returns>

        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] CreateNotificationRequest request)
        {
            try
            {
                await _notificationService.AddNotification(request);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ValidationException e)
            {
                return StatusCode(StatusCodes.Status412PreconditionFailed, e.ToString());
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Belirli bir kullan�c�n�n bildirimlerini getirir.
        /// </summary>
        /// <param name="userId">Kullan�c� kimli�i</param>
        /// <returns>HTTP 200 OK ve kullan�c�n�n bildirimleri veya uygun bir hata durumu</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(int userId)
        {
            //Bu y�ntem, kullan�c�n�n kimli�i userId ile belirtilen kullan�c�n�n bildirimlerini getirir.Kullan�c� bulunamazsa 404 Not Found durumu d�ner. Di�er hatalar i�in ise 500 Internal Server Error d�ner.
            try
            {
                var notifications = await _notificationService.GetByUserId(userId);

                return StatusCode(StatusCodes.Status200OK, notifications);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Belirtilen sipari� kimli�i ile ilgili bildirimleri getirir.
        /// </summary>
        /// <param name="id">Sipari� kimli�i</param>
        /// <returns>HTTP 200 OK ve bildirimler veya uygun bir hata durumu</returns>
        [HttpGet("instruction/{id}")]
        public async Task<IActionResult> GetNotificaitonByInstructionsId(int id)
        {
            //Bu y�ntem, belirtilen sipari� kimli�i id ile ilgili bildirimleri getirir.E�er sipari� bulunamazsa 404 Not Found durumu d�ner. Di�er hatalar i�in ise 500 Internal Server Error d�ner.
            try
            {
                var notifications = await _notificationService.GetNotificaitonByInstructionId(id);

                return StatusCode(StatusCodes.Status200OK, notifications);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Belirtilen talimat kimli�i ile ilgili bildirimleri getirir.
        /// </summary>
        /// <param name="instructionId">Talimat kimli�i</param>
        /// <returns>HTTP 200 OK ve bildirim g�nl�kleri veya uygun bir hata durumu</returns>
        [HttpGet("instruction/{instructionId}/logs")]
        public async Task<IActionResult> GetNotificationLogsByInstructionId(int instructionId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationLogsByInstructionId(instructionId);

                return StatusCode(StatusCodes.Status200OK, notifications);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Belirtilen kullan�c� i�in bildirim g�nl�klerini getirir.
        /// </summary>
        /// <param name="userId">Kullan�c� kimli�i</param>
        /// <returns>HTTP 200 OK ve bildirim g�nl�kleri veya uygun bir hata durumu</returns>

        [HttpGet("user/{userId}/logs")]
        public async Task<IActionResult> GetNotificationLogsByUserId(int userId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationLogsByUserId(userId);

                return StatusCode(StatusCodes.Status200OK, notifications);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Belirtilen kullan�c� i�in bildirim g�nderir.
        /// </summary>
        /// <param name="userId">Kullan�c� kimli�i</param>
        /// <returns>HTTP 200 OK veya uygun bir hata durumu</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] int userId)
        {
            try
            {
                await _notificationService.SendNotificationByUserId(userId);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

    }
}