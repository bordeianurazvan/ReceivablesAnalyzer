using AutoMapper;
using Ingestion.Application.Models;
using Ingestion.Domain.Entities;

namespace Ingestion.Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<InvoiceDto, Invoice>();
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(d => d.IssueDate, opt => opt.MapFrom(x => x.IssueDate.ToString("yyyy-MM-dd")))
            .ForMember(d => d.DueDate, opt => opt.MapFrom(x => x.DueDate.ToString("yyyy-MM-dd")))
            .ForMember(d => d.ClosedDate, opt => opt.MapFrom(x => MapNullableDateTimeOffsetToString(x.ClosedDate)));

        CreateMap<CreditNoteDto, CreditNote>().ReverseMap();
        CreateMap<CreditNote, CreditNoteDto>()
            .ForMember(d => d.IssueDate, opt => opt.MapFrom(x => x.IssueDate.ToString("yyyy-MM-dd")))
            .ForMember(d => d.DueDate, opt => opt.MapFrom(x => x.DueDate.ToString("yyyy-MM-dd")))
            .ForMember(d => d.ClosedDate, opt => opt.MapFrom(x => MapNullableDateTimeOffsetToString(x.ClosedDate)));

    }

    private static string? MapNullableDateTimeOffsetToString(DateTimeOffset? dateTimeOffset)
    {
        return dateTimeOffset?.ToString("yyyy-MM-dd");
    }
}
