using Ingestion.Application.Models;
using Ingestion.Domain.Entities;

namespace Ingestion.Application.Services;

public interface ICreditNoteService
{
    Task<IList<CreditNoteDto>?>? GetAllAsync();
    Task<CreditNoteDto?> GetByReferenceAsync(string reference);
    Task<IList<CreditNoteDto>> InsertAsync(IList<CreditNoteDto> creditNoteDto);
    Task<CreditNoteDto> UpdateAsync(CreditNoteDto creditNoteDto);
    Task<bool> DeleteAsync(string reference);
}
