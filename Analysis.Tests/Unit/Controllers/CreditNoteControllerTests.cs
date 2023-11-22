using Analysis.Api.Controllers;
using Analysis.Application.Models;
using Analysis.Application.Services;
using Analysis.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Analysis.Tests.Unit.Controllers;

public class CreditNotesControllerTests
{
    private Mock<ILogger<CreditNoteController>>? _loggerMock;
    private Mock<ICreditNoteService>? _creditNoteServiceMock;

    private CreditNoteController sut;

    private IList<CreditNote> _creditNotes = new List<CreditNote>
    {
        new CreditNote
        {
            Reference = "74283561-ba83-43b2-91da-3b2444cd44aa",
            CurrencyCode = "EUR",
            IssueDate = DateTimeOffset.Parse("2023-11-01"),
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
        }
    };

    public CreditNotesControllerTests()
    {
        _loggerMock = new Mock<ILogger<CreditNoteController>>();
        _creditNoteServiceMock = new Mock<ICreditNoteService>();

        sut = new CreditNoteController(_loggerMock.Object, _creditNoteServiceMock.Object);
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new CreditNoteController(null!, _creditNoteServiceMock!.Object));
    }

    [Fact]
    public void Constructor_WithNullService_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new CreditNoteController(_loggerMock!.Object, null!));
    }

    [Fact]
    public async Task GetAll_WhenDataExists_ReturnsOk()
    {
        // Arrange
        var expectedCreditNoteCount = _creditNotes.Count;
        _creditNoteServiceMock!.Setup(x => x.GetAllCreditNotesAsync())!.ReturnsAsync(_creditNotes);

        // Act
        var response = await sut.Get();

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var creditNotes = Assert.IsType<List<CreditNote>>(result.Value);
        Assert.Equal(expectedCreditNoteCount, creditNotes.Count);
    }

    [Fact]
    public async Task GetAll_WhenDataDoesNotExists_ReturnsNotFound()
    {
        // Arrange
        _creditNoteServiceMock!.Setup(x => x.GetAllCreditNotesAsync())!.ReturnsAsync((List<CreditNote>)null!);

        // Act
        var response = await sut.Get();

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<NotFoundResult>(response);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GetByReference_WhenValidReferenceAndDataExists_ReturnsOk()
    {
        // Arrange
        var expectedCreditNote = _creditNotes.First();
        var expectedReference = expectedCreditNote.Reference;
        _creditNoteServiceMock!.Setup(x => x.GetCreditNoteByReferenceAsync(expectedReference))!.ReturnsAsync(expectedCreditNote);

        // Act
        var response = await sut.Get(expectedReference);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var creditNote = Assert.IsType<CreditNote>(result.Value);
        Assert.Equal(expectedReference, creditNote.Reference);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetByReference_WhenInvalidReference_ReturnsBadRequest(string? reference)
    {
        // Arrange
        _creditNoteServiceMock!.Setup(x => x.GetCreditNoteByReferenceAsync(It.IsAny<string>()))!.ReturnsAsync(It.IsAny<CreditNote>());

        // Act
        var response = await sut.Get(reference);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task GetSummary_WhenInvalidStartDate_ReturnsBadRequest()
    {
        // Arrange
        var startDate = DateTimeOffset.Parse("2023-11-02");
        var endDate = DateTimeOffset.Parse("2023-11-01");
        _creditNoteServiceMock!.Setup(x => x.GetSummaryCreditNoteAsync(startDate, endDate, null, null))!.ReturnsAsync(It.IsAny<SummaryCreditNote>);

        // Act
        var response = await sut.Get(startDate, endDate, null, null);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task GetSummary_WhenValidParameters_ReturnsOk()
    {
        // Arrange
        var expectedStartDate = _creditNotes.First().IssueDate;
        var expectedEndDate = _creditNotes.First().IssueDate;
        var expectedIncludeClosedCreditNotes = true;
        var expectedIncludeOpenCreditNotes = false;
        var expectedSummaryCreditNote = new SummaryCreditNote
        {
            StartDate = expectedStartDate,
            EndDate = expectedEndDate,
            IncludeClosedCreditNotes = expectedIncludeClosedCreditNotes,
            IncludeOpenCreditNotes = expectedIncludeOpenCreditNotes,
            CreditNotes = _creditNotes
        };
        _creditNoteServiceMock!.Setup(x => x.GetSummaryCreditNoteAsync(expectedStartDate, expectedEndDate, expectedIncludeClosedCreditNotes, expectedIncludeOpenCreditNotes))!.ReturnsAsync(expectedSummaryCreditNote);

        // Act
        var response = await sut.Get(expectedStartDate, expectedEndDate, expectedIncludeClosedCreditNotes, expectedIncludeOpenCreditNotes);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var summaryCreditNoteResult = Assert.IsType<SummaryCreditNote>(result.Value);
        Assert.Equal(expectedStartDate, summaryCreditNoteResult.StartDate);
        Assert.Equal(expectedEndDate, summaryCreditNoteResult.EndDate);
        Assert.Equal(expectedIncludeClosedCreditNotes, summaryCreditNoteResult.IncludeClosedCreditNotes);
        Assert.Equal(expectedIncludeOpenCreditNotes, summaryCreditNoteResult.IncludeOpenCreditNotes);
        Assert.Equal(expectedSummaryCreditNote.CreditNotes.Count, summaryCreditNoteResult.CreditNotes.Count);
    }


    [Fact]
    public async Task GetSummary_WhenNoData_ReturnsNotFound()
    {
        // Arrange
        _creditNoteServiceMock!.Setup(x => x.GetSummaryCreditNoteAsync(null, null, null, null))!.ReturnsAsync((SummaryCreditNote)null!);

        // Act
        var response = await sut.Get(null, null, null, null);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<NotFoundResult>(response);
        Assert.Equal(404, result.StatusCode);
    }
}
