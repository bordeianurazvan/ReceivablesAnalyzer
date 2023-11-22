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
