using Analysis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Analysis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CreditNoteController : ControllerBase
{
    private readonly ILogger<CreditNoteController> _logger;
    private readonly ICreditNoteService _creditNoteService;

    public CreditNoteController(ILogger<CreditNoteController> logger, ICreditNoteService creditNoteService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _creditNoteService = creditNoteService ?? throw new ArgumentNullException(nameof(creditNoteService));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var creditNotes = await _creditNoteService.GetAllCreditNotesAsync();
        if (creditNotes == null)
        {
            return NotFound();
        }

        return Ok(creditNotes);
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> Get(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest("Reference can not be null or empty!");
        }

        var creditNote = await _creditNoteService.GetCreditNoteByReferenceAsync(reference);
        if (creditNote == null)
        {
            return NotFound();
        }

        return Ok(creditNote);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedInvoices, bool? includeOpenInvoices)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            if (startDate.Value > endDate.Value)
            {
                return BadRequest("Start date cannot be greater than end date.");
            }
        }
        var summaryCreditNote = await _creditNoteService.GetSummaryCreditNoteAsync(startDate, endDate, includeClosedInvoices, includeOpenInvoices);
        if (summaryCreditNote == null)
        {
            return NotFound();
        }

        return Ok(summaryCreditNote);
    }
}
