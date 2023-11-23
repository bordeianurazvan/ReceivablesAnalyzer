using Ingestion.Application.Models;
using Ingestion.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ingestion.Api.Controllers;

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
    /// Get all stored credit notes.
    /// </summary>
    /// <response code="200">Returns all stored credit notes.</response>
    /// <response code="404">If no credit notes are found.</response>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var creditNotes = await _creditNoteService.GetAllAsync()!;
        if (creditNotes == null)
        {
            return NotFound();
        }

        return Ok(creditNotes);
    }

    [HttpGet("{reference}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Get(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return BadRequest();
        }

        var creditNote = await _creditNoteService.GetByReferenceAsync(reference);
        if (creditNote == null)
        {
            return NotFound();
        }

        return Ok(creditNote);
    }

    /// <summary>
    /// Add new credit notes.
    /// </summary>
    /// <param name="creditNotes"></param>
    /// <returns>New added credit notes.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /CreditNote
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
    /// <response code="200">Returns a list of newly added credit notes.</response>
    /// <response code="400">If the provided list of credit notes is null</response>
    /// <response code="400">If the credit note payload is invalid</response>
    [HttpPost]
    public async Task<IActionResult> Post(IList<CreditNoteDto> creditNotes)
    {
        if (creditNotes == null)
        {
            return BadRequest("CreditNote payload cannot be null!");
        }

        var response = await _creditNoteService.InsertAsync(creditNotes);
        if (response == null || !response.Any())
        {
            return BadRequest("CreditNote payload is invalid!");
        }

        return Ok(response);
    }

    [HttpPut]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Update(CreditNoteDto creditNoteDto)
    {
        if (creditNoteDto == null)
        {
            return BadRequest();
        }

        var response = await _creditNoteService.UpdateAsync(creditNoteDto);
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

        var response = await _creditNoteService.DeleteAsync(reference);
        if (response)
        {
            return Ok(response);
        }

        return NotFound();
    }
}
