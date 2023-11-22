using Analysis.Domain.Entities;

namespace Analysis.Application.Models;

public class SummaryCreditNote
{
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    public bool? IncludeOpenCreditNotes { get; set; }
    public bool? IncludeClosedCreditNotes { get; set; }

    public double TotalAmount { get; set; }

    public IList<CreditNote> CreditNotes { get; set; }

}
