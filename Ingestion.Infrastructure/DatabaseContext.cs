using Ingestion.Domain.Configurations;
using Ingestion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingestion.Infrastructure
{
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
}
