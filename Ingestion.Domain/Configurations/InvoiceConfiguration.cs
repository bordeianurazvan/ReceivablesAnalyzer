using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ingestion.Domain.Entities;

namespace Ingestion.Domain.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(e => e.Reference);
        builder.Property(e => e.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(e => e.IssueDate).IsRequired();
        builder.Property(e => e.OpeningValue).IsRequired();
        builder.Property(e => e.PaidValue).IsRequired();
        builder.Property(e => e.DueDate).IsRequired();

        builder.Property(e => e.DebtorName).IsRequired();
        builder.Property(e => e.DebtorReference).IsRequired();
        builder.Property(e => e.DebtorReference).IsRequired();
        builder.Property(e => e.DebtorCountryCode).HasMaxLength(2).IsRequired();
    }
}
