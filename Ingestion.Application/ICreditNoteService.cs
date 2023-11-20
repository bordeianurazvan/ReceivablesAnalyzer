using Ingestion.Domain.Entities;

namespace Ingestion.Application;

public interface ICreditNoteService
{
    Task<IList<CreditNote>> GetAllAsync();
    Task<CreditNote?> GetByReferenceAsync(string reference);
    Task<CreditNote> InsertAsync(CreditNote entity);
    Task<CreditNote> UpdateAsync(CreditNote entity);
    Task<bool> DeleteAsync(string reference);
}
