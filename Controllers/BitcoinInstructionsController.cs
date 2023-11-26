using RecurringBitcoinPurchaseInstructions.Models;
using Microsoft.AspNetCore.Mvc;
using RecurringBitcoinPurchaseInstructions.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Transactions;
using RecurringBitcoinPurchaseInstructions.Data;

namespace RecurringBitcoinPurchaseInstructions.Controllers
{
    [ApiController]
    [Route("api/v1/instructions")]
    public class BitcoinInstructionsController : ControllerBase
    {
        private readonly ILogger<BitcoinInstructionsController> _logger;
        private readonly IInstructionService _instructionService;

        public BitcoinInstructionsController(ILogger<BitcoinInstructionsController> logger,
            IInstructionService instructionService)
        {
            _logger = logger;
            _instructionService = instructionService;
        }
        /// <summary>
        /// Talimat eklemek için gerekli iþlemler burada yapýlýr.
        /// </summary>     
        [HttpPost]
   
        public async Task<IActionResult> AddInstruction([FromBody] CreateInstructionsRequest request)
        {
            try
            {
                var instruction = await _instructionService.AddInstruction(request);

                return StatusCode(StatusCodes.Status201Created, instruction);
            }
            catch (ValidationException e)
            {
                return StatusCode(StatusCodes.Status412PreconditionFailed, e.ToString());
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.ToString());
            }
            catch (ConstraintException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.ToString());
            }
            catch (TransactionAbortedException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Talimat görüntüleme iþlemi burada yapýlýr.
        /// </summary>
        /// <param name="id"></param>

        [HttpGet("{id}")]        
        public async Task<IActionResult> GetInstructionById(int id)
        {
            try
            {
                var instruction = await _instructionService.GetInstructionById(id);

                return StatusCode(StatusCodes.Status200OK, instruction);
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
        /// Belirli bir kullanýcýnýn pasif talimatlarýný veya belirli bir miktarý kullanarak filtreleyerek getirir.
        /// </summary>
        /// <param name="userId">Talimatlarý filtrelemek için kullanýcý ID'si (isteðe baðlý).</param>
        /// <param name="amount">Talimatlarý belirli bir miktarla filtrelemek için (isteðe baðlý).</param>
        /// <returns>Pasif talimatlar listesi veya uygun hata durumu.</returns>
        [HttpGet("inactiveInstructions")]

        public async Task<IActionResult> GetInactiveInstructionsByUserAndAmount(int? userId, decimal? amount)
        {
            try
            {
                var instruction = await _instructionService.GetInactiveInstructionsByUserAndAmount(userId, amount);

                return StatusCode(StatusCodes.Status200OK, instruction);
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
        ///  Kullanýcýnýn talimatýný iptal etme iþlemi burda yapýlýr.
        /// </summary>
        /// <param name="id"></param>
        [HttpPatch("{id}/cancel")]       
        public async Task<IActionResult> CancelInstruction(int id)
        {
            try
            {
                await _instructionService.CancelInstruction(id);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.ToString());
            }
            catch (TransactionAbortedException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}