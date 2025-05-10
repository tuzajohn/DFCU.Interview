using DFCU.Interview.Domain.Enums;
using DFCU.Interview.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DFCU.Interview.API.Data;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {

    }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Payer)
            .HasMaxLength(10)
            .HasAnnotation("CheckConstraint", "CK_Transaction_Payer_OnlyDigits CHECK (Payer NOT LIKE '%[^0-9]%')");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Payee)
            .HasMaxLength(10)
            .HasAnnotation("CheckConstraint", "CK_Transaction_Payee_OnlyDigits CHECK (Payee NOT LIKE '%[^0-9]%')");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.PaymentStatus)
            .HasConversion(
                v => v.ToString(),
                v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v));

        modelBuilder.Entity<Transaction>()
            .Property(t => t.CreatedOn)
            .HasDefaultValueSql("GETDATE()");
    }
}
