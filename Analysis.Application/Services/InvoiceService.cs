using Analysis.Application.Models;
using Analysis.Domain.Entities;
using Analysis.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Analysis.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
    }

    public async Task<IList<Invoice>> GetAllInvoicesAsync()
    {
        return await _invoiceRepository.GetAllAsync();
    }

    public async Task<Invoice?> GetInvoiceByReferenceAsync(string reference)
    {
        return await _invoiceRepository.GetByReferenceAsync(reference);
    }

    public async Task<SummaryInvoice> GetSummaryInvoiceAsync(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedInvoices, bool? includeOpenInvoices)
    {
        var query = _invoiceRepository.GetAllQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(i => i.IssueDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(i => i.IssueDate <= endDate.Value);
        }

        if (includeOpenInvoices.HasValue || includeClosedInvoices.HasValue)
        {
            if (includeOpenInvoices.GetValueOrDefault() && !includeClosedInvoices.GetValueOrDefault())
            {
                // Include only open invoices
                query = query.Where(i => i.ClosedDate == null);
            }
            else if (!includeOpenInvoices.GetValueOrDefault() && includeClosedInvoices.GetValueOrDefault())
            {
                // Include only closed invoices
                query = query.Where(i => i.ClosedDate != null);
            }
            // If both are true, both are false, or both are null, do not include in the query
        }

        // Calculate the total amount directly in the database
        var amount = await query.SumAsync(i => i.OpeningValue);

        // Create the summary
        var summaryInvoice = new SummaryInvoice
        {
            StartDate = startDate,
            EndDate = endDate,
            IncludeClosedInvoices = includeClosedInvoices,
            IncludeOpenInvoices = includeOpenInvoices,
            TotalAmount = amount,
            Invoices = query.ToList()
        };

        return summaryInvoice;
    }
}
