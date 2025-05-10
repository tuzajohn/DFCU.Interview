using System.ComponentModel.DataAnnotations;

namespace DFCU.Interview.API.Models;

public class PaymentRequest
{
    [Required(ErrorMessage = "Payer Account Number is required.")]
    [RegularExpression(@"^[\d]{10}$", ErrorMessage = "Payer Account Number must be numeric and 10 characters long.")]
    public required string Payer { get; set; }

    public string? PayerReference { get; set; }

    [Required(ErrorMessage = "Payee Account Number is required.")]
    [RegularExpression(@"^[\d]{10}$", ErrorMessage = "Payee Account Number must be numeric and 10 characters long.")]
    public required string Payee { get; set; }

    [Required(ErrorMessage = "A valid ISO Currency is required.")]
    public required string Currency { get; set; }

    public required decimal Amount { get; set; }
}
