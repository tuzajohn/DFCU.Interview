namespace DFCU.Interview.Domain.Enums;

[Flags]
public enum PaymentStatus
{
    Pending = 0,
    Successful = 1,
    Failed = 2
}
