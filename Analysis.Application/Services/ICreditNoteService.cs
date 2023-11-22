using Analysis.Application.Models;
using Analysis.Domain.Entities;

namespace Analysis.Application.Services;

public interface ICreditNoteService
{
    Task<IList<CreditNote>> GetAllCreditNotesAsync();
    Task<CreditNote?> GetCreditNoteByReferenceAsync(string reference);

    Task<SummaryCreditNote> GetSummaryCreditNoteAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedCreditNotes, bool? includeOpenCreditNotes);
}
