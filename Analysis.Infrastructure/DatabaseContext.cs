using Analysis.Domain.Configurations;
using Analysis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Analysis.Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<CreditNote> CreditNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new CreditNoteConfiguration());
    }
}
