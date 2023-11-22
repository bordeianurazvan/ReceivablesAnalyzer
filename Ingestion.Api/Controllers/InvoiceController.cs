﻿using Ingestion.Application.Models;
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

    [HttpPost]
    public async Task<IActionResult> Post(IList<InvoiceDto> invoiceDtos)
    {
        if (invoiceDtos == null)
        {
            return BadRequest("Invoice payload cannot be null!");
        }

        var response = await _invoiceService.InsertAsync(invoiceDtos);
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
