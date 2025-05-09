using DFCU.Interview.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFCU.Interview.Domain.Models;

public class Transaction : BaseModel<Guid>
{
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? AccountNumber { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? Narration { get; set; }
}
