using AutoMapper;
using FluentResults;
using Ingestion.Application.Models;
using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;

namespace Ingestion.Application.Services;

public class CreditNoteService : ICreditNoteService
{
    private readonly ICreditNoteRepository _creditNoteRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreditNoteService> _logger;

    public CreditNoteService(ICreditNoteRepository creditNoteRepository, IMapper mapper, ILogger<CreditNoteService> logger)
    {
        _creditNoteRepository = creditNoteRepository ?? throw new ArgumentNullException(nameof(creditNoteRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IList<CreditNoteDto>?>? GetAllAsync()
    {
        IList<CreditNote>? creditNotes = await _creditNoteRepository.GetAllAsync();
        if (creditNotes != null && creditNotes?.Count > 0)
        {
            return _mapper.Map<IList<CreditNoteDto>>(creditNotes);
        }

        return null;
    }

    public async Task<CreditNoteDto?> GetByReferenceAsync(string reference)
    {
        CreditNote? creditNote = await _creditNoteRepository.GetByReferenceAsync(reference);
        if (creditNote == null)
        {
            return _mapper.Map<CreditNoteDto>(creditNote);
        }
        return null;
    }

    public async Task<IList<CreditNoteDto>> InsertAsync(IList<CreditNoteDto> creditNoteDtos)
    {
        var validCreditNotes = new List<CreditNote>();
        var invalidCreditNoteResults = Result.Ok();

        IList<CreditNote> creditNotes = _mapper.Map<IList<CreditNote>>(creditNoteDtos);
        foreach (var creditNote in creditNotes)
        {
            var result = creditNote.Validate();
            if (result.IsSuccess)
            {
                validCreditNotes.Add(creditNote);
            }
            else
            {
                invalidCreditNoteResults = Result.Merge(invalidCreditNoteResults, result);
            }
        }

        await _creditNoteRepository.InsertBulkAsync(validCreditNotes);
        _logger.LogWarning(string.Join(",", invalidCreditNoteResults.Errors.Select(e => e.Message)));

        return _mapper.Map<IList<CreditNoteDto>>(validCreditNotes);
    }

    public async Task<CreditNoteDto> UpdateAsync(CreditNoteDto creditNoteDto)
    {
        CreditNote creditNote = _mapper.Map<CreditNote>(creditNoteDto);
        await _creditNoteRepository.UpdateAsync(creditNote);
        return creditNoteDto;
    }

    public async Task<bool> DeleteAsync(string reference)
    {
        return await _creditNoteRepository.DeleteAsync(reference);
    }
}