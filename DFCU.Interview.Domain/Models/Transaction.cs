using DFCU.Interview.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DFCU.Interview.Domain.Models;

public class Transaction : BaseModel<Guid>
{
    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    [StringLength(10, MinimumLength = 10)]
    public string? Payer { get; set; }

    [StringLength(10, MinimumLength = 10)]
    public string? Payee { get; set; }

    public string? Currency { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    [StringLength(256)]
    public string? Narration { get; set; }
}
