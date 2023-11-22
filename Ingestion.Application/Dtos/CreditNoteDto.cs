using Ingestion.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Ingestion.Application.Models;

public class CreditNoteDto
{
    [Required]
    public string Reference { get; set; }

    [Required]
    [MinLength(3), MaxLength(3)]
    public string CurrencyCode { get; set; }

    [Required]
    [DateTimeValidation("yyyy-MM-dd")]
    public string IssueDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double OpeningValue { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double PaidValue { get; set; }

    [Required]
    [DateTimeValidation("yyyy-MM-dd")]
    public string DueDate { get; set; }

    [DateTimeValidation("yyyy-MM-dd")]
    public string? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }


    [Required]
    public string DebtorName { get; set; }

    [Required]
    public string DebtorReference { get; set; }

    [Required]
    [MinLength(2), MaxLength(2)]
    public string DebtorCountryCode { get; set; }
    public string? DebtorAddress1 { get; set; }
    public string? DebtorAddress2 { get; set; }
    public string? DebtorTown { get; set; }
    public string? DebtorState { get; set; }
    public string? DebtorZip { get; set; }
    public string? DebtorRegistrationNumber { get; set; }

}
