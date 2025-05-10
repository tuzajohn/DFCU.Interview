using DFCU.Interview.Domain.Enums;
using System.Net;

namespace DFCU.Interview.API.Helpers.Utils;

public static class PaymentStatusCodeExtension
{
    public static HttpStatusCode GetPaymentStatusCode(this PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending => HttpStatusCode.Continue,
            PaymentStatus.Successful => HttpStatusCode.OK,
            PaymentStatus.Failed => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.Unused
        };
    }

    public static PaymentStatus GetRandomPaymentStatus(this Random _random, ILogger _logger)
    {
        var randomNumber = _random.Next(1, 100);
        
        _logger.LogInformation("Random number generated: {randomNumber}", randomNumber);

        return randomNumber switch
        {
            <= 5 => PaymentStatus.Failed,
            <= 10 => PaymentStatus.Pending,
            <= 85 => PaymentStatus.Successful,
            _ => PaymentStatus.Failed
        };
    }
}
