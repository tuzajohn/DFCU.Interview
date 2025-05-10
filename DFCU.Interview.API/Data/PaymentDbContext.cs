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
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.AccountNumber)
            .HasMaxLength(10);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Narration)
            .HasMaxLength(200);

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
