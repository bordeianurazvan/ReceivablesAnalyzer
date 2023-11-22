using Analysis.Application.Models;
using Analysis.Domain.Entities;
using Analysis.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Analysis.Application.Services;

public class CreditNoteService : ICreditNoteService
{
    private readonly ICreditNoteRepository _creditNoteRepository;

    public CreditNoteService(ICreditNoteRepository creditNoteRepository)
    {
        _creditNoteRepository = creditNoteRepository ?? throw new ArgumentNullException(nameof(creditNoteRepository));
    }

    public async Task<IList<CreditNote>> GetAllCreditNotesAsync()
    {
        return await _creditNoteRepository.GetAllAsync();
    }

    public async Task<CreditNote?> GetCreditNoteByReferenceAsync(string reference)
    {
        return await _creditNoteRepository.GetByReferenceAsync(reference);
    }

    public async Task<SummaryCreditNote> GetSummaryCreditNoteAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedCreditNotes, bool? includeOpenCreditNotes)
    {
        var query = _creditNoteRepository.GetAllQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(i => i.IssueDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(i => i.IssueDate <= endDate.Value);
        }

        if (includeOpenCreditNotes.HasValue || includeClosedCreditNotes.HasValue)
        {
            if (includeOpenCreditNotes.GetValueOrDefault() && !includeClosedCreditNotes.GetValueOrDefault())
            {
                // Include only open invoices
                query = query.Where(i => i.ClosedDate == null);
            }
            else if (!includeOpenCreditNotes.GetValueOrDefault() && includeClosedCreditNotes.GetValueOrDefault())
            {
                // Include only closed invoices
                query = query.Where(i => i.ClosedDate != null);
            }
            // If both are true, both are false, or both are null, do not include in the query
        }

        // Calculate the total amount directly in the database
        var amount = await query.SumAsync(i => i.OpeningValue);

        // Create the summary
        var summaryCreditNote = new SummaryCreditNote
        {
            StartDate = startDate,
            EndDate = endDate,
            IncludeClosedCreditNotes = includeClosedCreditNotes,
            IncludeOpenCreditNotes = includeOpenCreditNotes,
            TotalAmount = amount,
            CreditNotes = query.ToList()
        };

        return summaryCreditNote;
    }
}
