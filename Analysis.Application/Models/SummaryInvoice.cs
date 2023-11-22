using Analysis.Domain.Entities;

namespace Analysis.Application.Models;

public class SummaryInvoice
{
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    public bool? IncludeOpenInvoices { get; set; }
    public bool? IncludeClosedInvoices { get; set; }

    public double TotalAmount { get; set; }

    public IList<Invoice> Invoices { get; set; }

}
