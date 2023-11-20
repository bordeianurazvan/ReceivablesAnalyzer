using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;

namespace Ingestion.Application;

public class CreditNoteService : ICreditNoteService
{
    private readonly ICreditNoteRepository _creditNoteRepository;

    public CreditNoteService(ICreditNoteRepository creditNoteRepository)
    {
        _creditNoteRepository = creditNoteRepository ?? throw new ArgumentNullException(nameof(creditNoteRepository));
    }

    public async Task<IList<CreditNote>> GetAllAsync()
    {
        return await _creditNoteRepository.GetAllAsync();
    }

    public async Task<CreditNote?> GetByReferenceAsync(string reference)
    {
        return await _creditNoteRepository.GetByReferenceAsync(reference);
    }

    public async Task<CreditNote> InsertAsync(CreditNote entity)
    {
        return await _creditNoteRepository.InsertAsync(entity);
    }

    public async Task<CreditNote> UpdateAsync(CreditNote entity)
    {
        return await _creditNoteRepository.UpdateAsync(entity);
    }
    public async Task<bool> DeleteAsync(string reference)
    {
        return await _creditNoteRepository.DeleteAsync(reference);
    }
}
