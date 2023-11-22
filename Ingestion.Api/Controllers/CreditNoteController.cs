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

    [HttpPost]
    public async Task<IActionResult> Post(IList<CreditNoteDto> creditNotesDtos)
    {
        if (creditNotesDtos == null)
        {
            return BadRequest("CreditNote payload cannot be null!");
        }

        var response = await _creditNoteService.InsertAsync(creditNotesDtos);
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
