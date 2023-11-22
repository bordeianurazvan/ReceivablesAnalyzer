using Analysis.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Analysis.Domain.Configurations;

public class CreditNoteConfiguration : IEntityTypeConfiguration<CreditNote>
{
    public void Configure(EntityTypeBuilder<CreditNote> builder)
    {
        builder.HasKey(e => e.Reference);
        builder.Property(e => e.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(e => e.IssueDate).IsRequired();
        builder.Property(e => e.OpeningValue).IsRequired();
        builder.Property(e => e.PaidValue).IsRequired();
        builder.Property(e => e.DueDate).IsRequired();

        builder.Property(e => e.DebtorName).IsRequired();
        builder.Property(e => e.DebtorReference).IsRequired();
        builder.Property(e => e.DebtorCountryCode).HasMaxLength(2).IsRequired();
    }
}
