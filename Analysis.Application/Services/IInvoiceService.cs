using Analysis.Application.Models;
using Analysis.Domain.Entities;

namespace Analysis.Application.Services;

public interface IInvoiceService
{
    Task<IList<Invoice>> GetAllInvoicesAsync();
    Task<Invoice?> GetInvoiceByReferenceAsync(string reference);

    Task<SummaryInvoice> GetSummaryInvoiceAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedInvoices, bool? includeOpenInvoices);
}
