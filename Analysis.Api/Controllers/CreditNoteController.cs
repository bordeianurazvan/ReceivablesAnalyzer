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

    /// <summary>
    /// Get all credit notes.
    /// </summary>
    /// <response code="200">Returns all credit notes.</response>
    /// <response code="404">If no credit notes are found.</response>
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

    /// <summary>
    /// Get credit note by reference.
    /// </summary>
    /// <param name="reference">Unique reference of an credit note.</param>
    /// <response code="200">Returns corresponding credit note.</response>
    /// <response code="404">If no credit note is found.</response>
    /// <response code="400">If provided reference is null or empty.</response>
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

    /// <summary>
    /// Get summary about credit notes. The amount(of opening values) for open and/or closed credit notes. 
    /// </summary>
    /// <param name="startDate">Start date from where we filter the credit notes.</param>
    /// <param name="endDate">End date until we filter the credit notes.</param>
    /// <param name="includeClosedCreditNotes">Include closed credit notes.</param>
    /// <param name="includeOpenCreditNotes">Include open credit notes.</param>
    /// <returns>Summary about credit notes based on the input.</returns>
    /// <remarks>
    /// Sample input:
    ///
    ///     StartDate: 2023-11-01
    ///     EndDate: 2023-11-30
    ///     IncludeClosedCreditNotes: True
    ///     IncludeOpenCreditNotes: True
    ///     
    /// format of the DateTime is "yyyy-mm-dd".
    /// if both (open and close inputs) are true, both are false, or both are null, include all data in the summary.
    /// </remarks>
    /// <response code="200">Returns summary about credit notes</response>
    /// <response code="400">If any input parameter is wrong.</response>
    /// <response code="404">If no credit note is found.</response>
    [HttpGet("summary")]
    public async Task<IActionResult> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, bool? includeClosedCreditNotes, bool? includeOpenCreditNotes)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            if (startDate.Value > endDate.Value)
            {
                return BadRequest("Start date cannot be greater than end date.");
            }
        }
        var summaryCreditNote = await _creditNoteService.GetSummaryCreditNoteAsync(startDate, endDate, includeClosedCreditNotes, includeOpenCreditNotes);
        if (summaryCreditNote == null)
        {
            return NotFound();
        }

        return Ok(summaryCreditNote);
    }
}
