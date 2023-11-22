using Ingestion.Application.Models;

namespace Ingestion.Application.Services;

public interface IInvoiceService
{
    Task<IList<InvoiceDto>?>? GetAllAsync();
    Task<InvoiceDto?> GetByReferenceAsync(string reference);
    Task<IList<InvoiceDto>> InsertAsync(IList<InvoiceDto> invoiceDtos);
    Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto); 
    Task<bool> DeleteAsync(string reference);
}
