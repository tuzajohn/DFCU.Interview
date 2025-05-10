using DFCU.Interview.API.Data;
using DFCU.Interview.API.Helpers.Utils;
using DFCU.Interview.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DFCU.Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentDbContext _dbContext;
        private readonly ILogger<PaymentsController> _logger;
        readonly Random _random = new Random();
        public PaymentsController(PaymentDbContext dbContext, ILogger<PaymentsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _random.Next(1, 100);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var transactions = _dbContext.Transactions.ToList();
            
            return Ok(transactions);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            if (id is null || id == Guid.Empty)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Transaction not found.",
                });
            }

            var transaction = await _dbContext.Transactions.FindAsync(id);

            if (transaction is null)
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Transaction not found.",
                });
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Aggregate(string.Empty, (current, error) => current + error + "\n");


                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = $"Transaction failed {errorMessage}",
                });
            }

            if (request.Payer.Equals(request.Payee, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Payer and Payee cannot be the same.",
                });
            }

            var transaction = new Domain.Models.Transaction
            {
                Amount = request.Amount,
                Payer = request.Payer,
                Payee = request.Payee,
                Narration = request.PayerReference,
                TransactionDate = DateTime.UtcNow,
                Currency = request.Currency,
                PaymentStatus = _random.GetRandomPaymentStatus(_logger)
            };

            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, request);
        }
    }
}
