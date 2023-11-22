﻿namespace Analysis.Domain.Entities;

public class CreditNote
{
    public string Reference { get; set; }
    public string CurrencyCode { get; set; }
    public DateTimeOffset IssueDate { get; set; }
    public double OpeningValue { get; set; }
    public double PaidValue { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public DateTimeOffset? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }

    public string DebtorName { get; set; }
    public string DebtorReference { get; set; }
    public string? DebtorAddress1 { get; set; }
    public string? DebtorAddress2 { get; set; }
    public string? DebtorTown { get; set; }
    public string? DebtorState { get; set; }
    public string? DebtorZip { get; set; }
    public string DebtorCountryCode { get; set; }
    public string? DebtorRegistrationNumber { get; set; }
}
