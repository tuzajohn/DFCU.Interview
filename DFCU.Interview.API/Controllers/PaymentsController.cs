using DFCU.Interview.API.Data;
using DFCU.Interview.API.Helpers.Utils;
using DFCU.Interview.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;

namespace DFCU.Interview.API.Controllers;

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


    /// <summary>
    /// Retrieves a list of transactions from the database.
    /// </summary>
    /// <returns>Returns an HTTP 200 response with the list of transactions.</returns>
    [HttpGet]
    public IActionResult Get()
    {
        var transactions = _dbContext.Transactions.ToList();

        return Ok(transactions);
    }

    /// <summary>
    /// Retrieves a transaction based on a unique identifier. Returns a not found response if the identifier is
    /// invalid or the transaction does not exist.
    /// </summary>
    /// <param name="id">The unique identifier used to locate a specific transaction in the database.</param>
    /// <returns>
    /// Returns the transaction data or not found in cases where the identifier is invalid or the transaction does not exist.
    /// </returns>
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

    [HttpGet]
    [Route("{id:guid}/status")]
    public async Task<IActionResult> FindPaymentStatus(Guid id)
    {
        var transaction = await _dbContext.Transactions.FindAsync(id);
        if (transaction is null)
        {
            return NotFound(new
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Transaction not found.",
            });
        }

        return Ok(transaction.PaymentStatus.ToString());
    }

    /// <summary>
    /// Handles payment transactions by processing a payment request and saving it to the database.
    /// </summary>
    /// <param name="request">Contains details about the payment, including payer, payee, amount, and currency.</param>
    /// <returns>Returns a response indicating the result of the transaction, including success or error messages.</returns>
    /// 
    [ProducesResponseType(typeof(PaymentRequest), StatusCodes.Status201Created)]
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
