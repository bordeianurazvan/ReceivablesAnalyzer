using Analysis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Analysis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(ILogger<InvoiceController> logger, IInvoiceService invoiceService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
    }

    /// <summary>
    /// Get all invoices.
    /// </summary>
    /// <response code="200">Returns all invoices.</response>
    /// <response code="404">If no invoices are found.</response>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var invoices = await _invoiceService.GetAllInvoicesAsync();
        if (invoices == null)
        {
            return NotFound();
        }

        return Ok(invoices);
    }

    /// <summary>
    /// Get invoice by reference.
    /// </summary>
    /// <param name="reference">Unique reference of an invoice.</param>
    /// <response code="200">Returns corresponding invoice.</response>
    /// <response code="404">If no invoice is found.</response>
    /// <response code="400">If provided reference is null or empty.</response>
    [HttpGet("{reference}")]
    public async Task<IActionResult> Get(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest("Reference can not be null or empty!");
        }

        var invoice = await _invoiceService.GetInvoiceByReferenceAsync(reference);
        if (invoice == null)
        {
            return NotFound();
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Get summary about invoices. The amount(of opening values) for open and/or closed invoices. 
    /// </summary>
    /// <param name="startDate">Start date from where we filter the invoices.</param>
    /// <param name="endDate">End date until we filter the invoices.</param>
    /// <param name="includeClosedInvoices">Include closed invoices.</param>
    /// <param name="includeOpenInvoices">Include open invoices.</param>
    /// <returns>Summary about invoices based on the input.</returns>
    /// <remarks>
    /// Sample input:
    ///
    ///     StartDate: 2023-11-01
    ///     EndDate: 2023-11-30
    ///     IncludeClosedInvoices: True
    ///     IncludeOpenInvoices: True
    ///     
    /// format of the DateTime is "yyyy-mm-dd".
    /// if both (open and close inputs) if both (open and close inputs) are true, both are false, or both are null, include all data in the summary.
    /// </remarks>
    /// <response code="200">Returns summary about invoices</response>
    /// <response code="400">If any input parameter is wrong.</response>
    /// <response code="404">If no invoice is found.</response>
    [HttpGet("summary")]
    public async Task<IActionResult> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedInvoices, bool? includeOpenInvoices)
    {
        if(startDate.HasValue && endDate.HasValue)
        {
            if(startDate.Value > endDate.Value)
            {
                return BadRequest("Start date cannot be greater than end date.");
            }
        }
        var summaryInvoice = await _invoiceService.GetSummaryInvoiceAsync(startDate, endDate, includeClosedInvoices, includeOpenInvoices);
        if (summaryInvoice == null)
        {
            return NotFound();
        }

        return Ok(summaryInvoice);
    }
}
