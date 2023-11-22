using Analysis.Application.Models;
using Analysis.Application.Services;
using Analysis.Domain.Entities;
using Analysis.Domain.RepositoryInterfaces;
using MockQueryable.Moq;
using Moq;

namespace Analysis.Tests.Unit.Services;

public class CreditNoteServiceTests
{
    private Mock<ICreditNoteRepository>? _creditNoteRepositoryMock;
    private CreditNoteService sut;


    private List<CreditNote> _creditNotes = new List<CreditNote>
    {
        new CreditNote
        {
            Reference = "74283561-ba83-43b2-91da-3b2444cd44aa",
            CurrencyCode = "EUR",
            IssueDate = DateTimeOffset.Parse("2023-11-01"),
            OpeningValue = 1001,
            PaidValue = 1000,
            DueDate = DateTimeOffset.Parse("2023-11-01"),
            ClosedDate = DateTimeOffset.Parse("2023-11-01"),
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
            DueDate = DateTimeOffset.Parse("2023-11-01"),
            ClosedDate = DateTimeOffset.Parse("2023-11-01"),
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

        sut = new CreditNoteService(_creditNoteRepositoryMock.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new CreditNoteService(null!));
    }

    [Fact]
    public async Task GetAllCreditNotesAsync_WhenDataExists_ReturnsData()
    {
        // Arrange
        var expectedCreditNoteCount = _creditNotes.Count;
        _creditNoteRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync(_creditNotes);

        // Act
        var result = await sut.GetAllCreditNotesAsync()!;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expectedCreditNoteCount, result.Count);
    }

    [Fact]
    public async Task GetAllCreditNotesAsync_WhenDataDoesNotExists_ReturnsNull()
    {
        // Arrange
        _creditNoteRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync((List<CreditNote>)null!);

        // Act
        var result = await sut.GetAllCreditNotesAsync()!;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCreditNoteByReferenceAsync_WhenDataExists_ReturnsData()
    {
        // Arrange
        var expectedCreditNote = _creditNotes.First();
        var expectedReference = expectedCreditNote.Reference;
        _creditNoteRepositoryMock!.Setup(x => x.GetByReferenceAsync(expectedReference)).ReturnsAsync(expectedCreditNote);

        // Act
        var result = await sut.GetCreditNoteByReferenceAsync(expectedReference)!;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedReference, result.Reference);
    }

    [Fact]
    public async Task GetSummaryCreditNoteAsync_WhenDataExists_ReturnsData()
    {
        // Arrange
        var expectedStartDate = _creditNotes.First().IssueDate;
        var expectedEndDate = _creditNotes.First().IssueDate;
        var expectedIncludeClosedCreditNotes = true;
        var expectedIncludeOpenCreditNotes = false;
        var expectedAmount = _creditNotes.Sum(i => i.OpeningValue);
        var expectedSummaryCreditNote = new SummaryCreditNote
        {
            StartDate = expectedStartDate,
            EndDate = expectedEndDate,
            IncludeClosedCreditNotes = expectedIncludeClosedCreditNotes,
            IncludeOpenCreditNotes = expectedIncludeOpenCreditNotes,
            TotalAmount = expectedAmount,
            CreditNotes = _creditNotes
        };

        var expectedCreditNotes = _creditNotes.BuildMock();
        _creditNoteRepositoryMock!.Setup(x => x.GetAllQueryable()).Returns(expectedCreditNotes);

        // Act
        var result = await sut.GetSummaryCreditNoteAsync(expectedStartDate, expectedEndDate, expectedIncludeClosedCreditNotes, expectedIncludeOpenCreditNotes)!;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSummaryCreditNote.CreditNotes.Count, result.CreditNotes.Count);
        Assert.Equal(expectedStartDate, result.StartDate);
        Assert.Equal(expectedEndDate, result.EndDate);
        Assert.Equal(expectedIncludeClosedCreditNotes, result.IncludeClosedCreditNotes);
        Assert.Equal(expectedIncludeOpenCreditNotes, result.IncludeOpenCreditNotes);
    }
}

