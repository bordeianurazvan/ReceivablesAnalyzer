using AutoMapper;
using FluentResults;
using Ingestion.Application.Models;
using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;

namespace Ingestion.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(IInvoiceRepository invoiceRepository, IMapper mapper, ILogger<InvoiceService> logger)
    {
        _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IList<InvoiceDto>?>? GetAllAsync()
    {
        IList<Invoice>? invoices = await _invoiceRepository.GetAllAsync();
        if (invoices != null && invoices?.Count > 0)
        {
            return _mapper.Map<IList<InvoiceDto>>(invoices);
        }

        return null;
    }

    public async Task<InvoiceDto?> GetByReferenceAsync(string reference)
    {
        Invoice? invoice = await _invoiceRepository.GetByReferenceAsync(reference);
        if (invoice == null)
        {
            return _mapper.Map<InvoiceDto>(invoice);
        }
        return null;
    }

    public async Task<IList<InvoiceDto>> InsertAsync(IList<InvoiceDto> invoiceDtos)
    {
        var validInvoices = new List<Invoice>();
        var invalidInvoicesResults = Result.Ok();

        IList<Invoice> invoices = _mapper.Map<IList<Invoice>>(invoiceDtos);
        foreach (var invoice in invoices)
        {
            var result = invoice.Validate();
            if (result.IsSuccess)
            {
                validInvoices.Add(invoice);
            }
            else
            {
                invalidInvoicesResults = Result.Merge(invalidInvoicesResults, result);
            }
        }

        await _invoiceRepository.InsertBulkAsync(validInvoices);
        if (invalidInvoicesResults.IsFailed)
        {
            _logger.LogWarning(string.Join(",", invalidInvoicesResults.Errors.Select(e => e.Message)));
        }

        return _mapper.Map<IList<InvoiceDto>>(validInvoices);
    }

    public async Task<InvoiceDto> UpdateAsync(InvoiceDto invoiceDto)
    {
        Invoice invoice = _mapper.Map<Invoice>(invoiceDto);
        await _invoiceRepository.UpdateAsync(invoice);
        return invoiceDto;
    }

    public async Task<bool> DeleteAsync(string reference)
    {
        return await _invoiceRepository.DeleteAsync(reference);
    }
}
