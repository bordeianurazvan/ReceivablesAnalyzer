using AutoMapper;
using Ingestion.Application.Models;
using Ingestion.Application.Services;
using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ingestion.Tests.Unit.Services;

public class CreditNoteServiceTests
{
    private Mock<ICreditNoteRepository>? _creditNoteRepositoryMock;
    private Mock<IMapper>? _mapperMock;
    private Mock<ILogger<CreditNoteService>>? _loggerMock;

    private CreditNoteService sut;

    private List<CreditNoteDto> _creditNoteDtos = new List<CreditNoteDto>
    {
        new CreditNoteDto
        {
            Reference = "74283561-ba83-43b2-91da-3b2444cd44aa",
            CurrencyCode = "EUR",
            IssueDate = "2023-11-01",
            OpeningValue = 1001,
            PaidValue = 1000,
            DueDate = "2023-11-30",
            ClosedDate = "2023-11-30",
            Cancelled = false,
            DebtorName = "ING BANK",
            DebtorReference = "3d811c09-c951-446e-a976-3cc176aaa28c",
            DebtorCountryCode = "RO",
            DebtorAddress1 = "Bucharest",
            DebtorAddress2 = "Random Street",
            DebtorTown = "Bucharest",
            DebtorState = "Romania",
            DebtorZip = "123456",
            DebtorRegistrationNumber = "1234567890"
        },
        new CreditNoteDto
        {
            Reference = "ec521953-a0d4-454f-a806-9b4418b3e3cc",
            CurrencyCode = "USD",
            IssueDate = "2023-11-01",
            OpeningValue = 500,
            PaidValue = 500,
            DueDate = "2023-11-30",
            ClosedDate = "2023-11-30",
            Cancelled = false,
            DebtorName = "ING BANK",
            DebtorReference = "3d811c09-c951-446e-a976-3cc176aaa28c",
            DebtorCountryCode = "RO",
            DebtorAddress1 = "Bucharest",
            DebtorAddress2 = "Random Street",
            DebtorTown = "Bucharest",
            DebtorState = "Romania",
            DebtorZip = "123456",
            DebtorRegistrationNumber = "1234567890"
        },
    };

    private List<CreditNote> _creditNotes = new List<CreditNote>
    {
        new CreditNote
        {
            Reference = "74283561-ba83-43b2-91da-3b2444cd44aa",
            CurrencyCode = "EUR",
            IssueDate = DateTimeOffset.Parse("2023-11-01"),
            OpeningValue = 1001,
            PaidValue = 1000,
            DueDate = DateTimeOffset.Parse("2023-11-02"),
            ClosedDate = DateTimeOffset.Parse("2023-11-02"),
            Cancelled = false,
            DebtorName = "ING BANK",
            DebtorReference = "3d811c09-c951-446e-a976-3cc176aaa28c",
            DebtorCountryCode = "RO",
            DebtorAddress1 = "Bucharest",
            DebtorAddress2 = "Random Street",
            DebtorTown = "Bucharest",
            DebtorState = "Romania",
            DebtorZip = "123456",
            DebtorRegistrationNumber = "1234567890"
        },
        new CreditNote
        {
            Reference = "ec521953-a0d4-454f-a806-9b4418b3e3cc",
            CurrencyCode = "USD",
            IssueDate = DateTimeOffset.Parse("2023-11-01"),
            OpeningValue = 500,
            PaidValue = 500,
            DueDate = DateTimeOffset.Parse("2023-11-02"),
            ClosedDate = DateTimeOffset.Parse("2023-11-02"),
            Cancelled = false,
            DebtorName = "ING BANK",
            DebtorReference = "3d811c09-c951-446e-a976-3cc176aaa28c",
            DebtorCountryCode = "RO",
            DebtorAddress1 = "Bucharest",
            DebtorAddress2 = "Random Street",
            DebtorTown = "Bucharest",
            DebtorState = "Romania",
            DebtorZip = "123456",
            DebtorRegistrationNumber = "1234567890"
        },
    };

    public CreditNoteServiceTests()
    {
        _creditNoteRepositoryMock = new Mock<ICreditNoteRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreditNoteService>>();

        sut = new CreditNoteService(_creditNoteRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new CreditNoteService(null!, _mapperMock!.Object, _loggerMock!.Object));
    }

    [Fact]
    public void Constructor_WithNullMapper_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new CreditNoteService(_creditNoteRepositoryMock!.Object, null!, _loggerMock!.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new CreditNoteService(_creditNoteRepositoryMock!.Object, _mapperMock!.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_WhenDataExists_ReturnsMappedData()
    {
        // Arrange
        var expectedCreditNoteCount = _creditNotes.Count;
        _creditNoteRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync(_creditNotes);
        _mapperMock!.Setup(x => x.Map<IList<CreditNoteDto>>(_creditNotes)).Returns(_creditNoteDtos);

        // Act
        var result = await sut.GetAllAsync()!;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expectedCreditNoteCount, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenDataDoesNotExists_ReturnsNull()
    {
        // Arrange
        var expectedCreditNoteCount = _creditNotes.Count;
        _creditNoteRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync((List<CreditNote>)null!);

        // Act
        var result = await sut.GetAllAsync()!;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertAsync_WhenValidInput_MapAndReturnInsertedData()
    {
        // Arrange
        var creditNoteDto = _creditNoteDtos.First();
        var creditNoteDtos = new List<CreditNoteDto>
        {
            creditNoteDto
        };

        var expectedMappedCreditNote = _creditNotes.First();
        IList<CreditNote> expectedMappedCreditNotes = new List<CreditNote>
        {
            expectedMappedCreditNote
        };
        _creditNoteRepositoryMock!.Setup(x => x.InsertAsync(expectedMappedCreditNote))!.ReturnsAsync(expectedMappedCreditNote);
        _mapperMock!.Setup(x => x.Map<IList<CreditNote>>(creditNoteDtos)).Returns(expectedMappedCreditNotes);
        _mapperMock!.Setup(x => x.Map<IList<CreditNoteDto>>(expectedMappedCreditNotes)).Returns(creditNoteDtos);

        // Act
        var result = await sut.InsertAsync(creditNoteDtos);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result!);

        var dto = result.FirstOrDefault();
        Assert.Equal(expectedMappedCreditNote.Reference, dto?.Reference);
    }

    [Fact]
    public async Task InsertAsync_WhenBulkInsertInvalidInput_ReturnsOnlyValidData()
    {
        // Arrange
        var validCreditNoteDto = _creditNoteDtos.First();
        var invalidCreditNoteDto = new CreditNoteDto
        {
            Reference = "74283561-ba83-43b2-91da-3b2444cd44aa",
            CurrencyCode = "EUR",
            IssueDate = "2023-12-31",
            OpeningValue = 1001,
            PaidValue = 1000,
            DueDate = "2023-11-30",
            ClosedDate = "2023-11-30",
            Cancelled = false,
            DebtorName = "ING BANK",
            DebtorReference = "3d811c09-c951-446e-a976-3cc176aaa28c",
            DebtorCountryCode = "RO",
            DebtorAddress1 = "Bucharest",
            DebtorAddress2 = "Random Street",
            DebtorTown = "Bucharest",
            DebtorState = "Romania",
            DebtorZip = "123456",
            DebtorRegistrationNumber = "1234567890"
        };
        var creditNoteDtos = new List<CreditNoteDto>
        {
            validCreditNoteDto,
            invalidCreditNoteDto
        };

        CreditNote expectedMappedValidCreditNote = _creditNotes.First();
        CreditNote expectedMappedInvalidCreditNote = new CreditNote
        {
            Reference = "74283561-ba83-43b2-91da-3b2444cd44aa",
            CurrencyCode = "EUR",
            IssueDate = DateTimeOffset.Parse("2023-12-31"),
            OpeningValue = 1001,
            PaidValue = 1000,
            DueDate = DateTimeOffset.Parse("2023-11-30"),
            ClosedDate = DateTimeOffset.Parse("2023-11-30"),
            Cancelled = false,
            DebtorName = "ING BANK",
            DebtorReference = "3d811c09-c951-446e-a976-3cc176aaa28c",
            DebtorCountryCode = "RO",
            DebtorAddress1 = "Bucharest",
            DebtorAddress2 = "Random Street",
            DebtorTown = "Bucharest",
            DebtorState = "Romania",
            DebtorZip = "123456",
            DebtorRegistrationNumber = "1234567890"
        };
        IList<CreditNote> mappedCreditNotes = new List<CreditNote>
        {
            expectedMappedValidCreditNote,
            expectedMappedInvalidCreditNote
        };

        IList<CreditNote> mappedValidCreditNotes = new List<CreditNote> { expectedMappedValidCreditNote };

        _creditNoteRepositoryMock!.Setup(x => x.InsertBulkAsync(mappedValidCreditNotes))!.ReturnsAsync(mappedValidCreditNotes);
        _mapperMock!.Setup(x => x.Map<IList<CreditNote>>(creditNoteDtos)).Returns(mappedCreditNotes);
        _mapperMock!.Setup(x => x.Map<IList<CreditNoteDto>>(mappedValidCreditNotes)).Returns(new List<CreditNoteDto> { validCreditNoteDto });

        // Act
        var result = await sut.InsertAsync(creditNoteDtos);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result!);
        Assert.Equal(mappedValidCreditNotes.Count, result.Count);

        var dto = result.FirstOrDefault();
        Assert.Equal(expectedMappedValidCreditNote.Reference, dto?.Reference);
    }

    //Unit tests for GetByReference, Update and Delete methods are skipped because will be similar with those from above
    //and they are out of the scope of the ingestion microservice as it's mentioned in the homework specifications
}
