using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;

namespace Ingestion.Application;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
    }

    public async Task<IList<Invoice>> GetAllAsync()
    {
        return await _invoiceRepository.GetAllAsync();
    }

    public async Task<Invoice?> GetByReferenceAsync(string reference)
    {
        return await _invoiceRepository.GetByReferenceAsync(reference);
    }

    public async Task<Invoice> InsertAsync(Invoice entity)
    {
        return await _invoiceRepository.InsertAsync(entity);
    }

    public async Task<Invoice> UpdateAsync(Invoice entity)
    {
        return await _invoiceRepository.UpdateAsync(entity);
    }
    public async Task<bool> DeleteAsync(string reference)
    {
        return await _invoiceRepository.DeleteAsync(reference);
    }
}
