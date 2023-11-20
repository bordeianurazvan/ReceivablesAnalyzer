using Ingestion.Domain.Entities;

namespace Ingestion.Application;

public interface IInvoiceService
{
    Task<IList<Invoice>> GetAllAsync();
    Task<Invoice?> GetByReferenceAsync(string reference);
    Task<Invoice> InsertAsync(Invoice entity);
    Task<Invoice> UpdateAsync(Invoice entity);
    Task<bool> DeleteAsync(string reference);
}
