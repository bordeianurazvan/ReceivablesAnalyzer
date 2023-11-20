using Ingestion.Application;
using Ingestion.Domain.Entities;
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
        _logger = logger;
        _invoiceService = invoiceService ?? throw new ArgumentNullException(nameof(invoiceService));
    }

    [HttpGet]
    public async Task<IEnumerable<Invoice>> Get()
    {
        return await _invoiceService.GetAllAsync();
    }

    [HttpGet("{reference}")]
    public async Task<Invoice> Get(string reference)
    {
        return await _invoiceService.GetByReferenceAsync(reference);
    }

    [HttpPost]
    public async Task<Invoice> Post(Invoice invoice)
    {
        return await _invoiceService.InsertAsync(invoice);
    }

    [HttpPut]
    public async Task<Invoice> Update(Invoice invoice)
    {
        return await _invoiceService.UpdateAsync(invoice);
    }

    [HttpDelete("{reference}")]
    public async Task<bool> Delete(string reference)
    {
        return await _invoiceService.DeleteAsync(reference);
    }
}
