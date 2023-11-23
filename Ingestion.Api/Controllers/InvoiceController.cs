using Ingestion.Application.Models;
using Ingestion.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ingestion.Api.Controllers;

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
    /// Get all stored invoices.
    /// </summary>
    /// <response code="200">Returns all stored invoices.</response>
    /// <response code="404">If no invoices are found.</response>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var invoices = await _invoiceService.GetAllAsync()!;
        if (invoices == null)
        {
            return NotFound();
        }

        return Ok(invoices);
    }

    [HttpGet("{reference}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Get(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest();
        }

        var invoice = await _invoiceService.GetByReferenceAsync(reference);
        if (invoice == null)
        {
            return NotFound();
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Add new invoices.
    /// </summary>
    /// <param name="invoices"></param>
    /// <returns>New added invoices</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Invoice
    ///     [
    ///       {
    ///         "reference": "74283561-ba83-43b2-91da-3b2444cd44aa",
    ///         "currencyCode": "EUR",
    ///         "issueDate": "2023-11-01",
    ///         "openingValue": 1001,
    ///         "paidValue": 1000,
    ///         "dueDate": "2023-11-30",
    ///         "closedDate": "2023-11-30",
    ///         "cancelled": false,
    ///         "debtorName": "Random Bank",
    ///         "debtorReference": "3d811c09-c951-446e-a976-3cc176aaa28c",
    ///         "debtorCountryCode": "RO",
    ///         "debtorAddress1": "Bucharest",
    ///         "debtorAddress2": "Random Street",
    ///         "debtorTown": "Bucharest",
    ///         "debtorState": "Romania",
    ///         "debtorZip": "123456",
    ///         "debtorRegistrationNumber": "1234567890"
    ///       }
    ///     ]
    ///
    /// </remarks>
    /// <response code="200">Returns a list of newly added invoices</response>
    /// <response code="400">If the provided list of invoices is null</response>
    /// <response code="400">If the invoice payload is invalid</response>
    [HttpPost]
    public async Task<IActionResult> Post(IList<InvoiceDto> invoices)
    {
        if (invoices == null)
        {
            return BadRequest("Invoice payload cannot be null!");
        }

        var response = await _invoiceService.InsertAsync(invoices);
        if (response == null || !response.Any())
        {
            return BadRequest("Invoice payload is invalid!");
        }

        return Ok(response);
    }

    [HttpPut]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Update(InvoiceDto invoiceDto)
    {
        if (invoiceDto == null)
        {
            return BadRequest();
        }

        var response = await _invoiceService.UpdateAsync(invoiceDto);
        return Ok(response);
    }

    [HttpDelete("{reference}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Delete(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest();
        }

        var response = await _invoiceService.DeleteAsync(reference);
        if (response)
        {
            return Ok(response);
        }

        return NotFound();
    }
}
