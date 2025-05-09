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
}
