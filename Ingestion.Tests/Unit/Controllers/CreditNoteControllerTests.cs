using Ingestion.Api.Controllers;
using Ingestion.Application.Models;
using Ingestion.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ingestion.Tests.Unit.Controllers;

public class CreditNoteControllerTests
{
    private Mock<ILogger<CreditNoteController>>? _loggerMock;
    private Mock<ICreditNoteService>? _creditNoteServiceMock;

    private CreditNoteController sut;

    private IList<CreditNoteDto> _creditNotes = new List<CreditNoteDto>
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

    public CreditNoteControllerTests()
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
    public async Task Get_WhenDataExists_ReturnsOk()
    {
        // Arrange
        var expectedCreditNotesCount = _creditNotes.Count;
        _creditNoteServiceMock!.Setup(x => x.GetAllAsync())!.ReturnsAsync(_creditNotes);

        // Act
        var response = await sut.Get();

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var CreditNoteDtos = Assert.IsType<List<CreditNoteDto>>(result.Value);
        Assert.Equal(expectedCreditNotesCount, CreditNoteDtos.Count);
    }

    [Fact]
    public async Task Get_WhenDataDoesNotExists_ReturnsNotFound()
    {
        // Arrange
        _creditNoteServiceMock!.Setup(x => x.GetAllAsync())!.ReturnsAsync((List<CreditNoteDto>)null!);

        // Act
        var response = await sut.Get();

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<NotFoundResult>(response);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Post_WhenValidInput_ReturnsOk()
    {
        // Arrange
        var expectedReference = Guid.NewGuid().ToString();
        var expectedCreditNote = new CreditNoteDto
        {
            Reference = expectedReference,
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
        };
        var expectedCreditNotes = new List<CreditNoteDto>
        {
            expectedCreditNote
        };

        _creditNoteServiceMock!.Setup(x => x.InsertAsync(expectedCreditNotes)).ReturnsAsync(expectedCreditNotes);

        // Act
        var response = await sut.Post(expectedCreditNotes);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var CreditNoteDtos = Assert.IsType<List<CreditNoteDto>>(result.Value);
        var CreditNoteDto = CreditNoteDtos.FirstOrDefault(x => x.Reference == expectedReference);
        Assert.NotNull(CreditNoteDto);
        Assert.Equal(expectedReference, CreditNoteDto.Reference);
    }

    [Fact]
    public async Task Post_WhenInvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var invalidInput = (List<CreditNoteDto>)null!;
        _creditNoteServiceMock!.Setup(x => x.InsertAsync(It.IsAny<List<CreditNoteDto>>())).ReturnsAsync((List<CreditNoteDto>)null!);

        // Act
        var response = await sut.Post(invalidInput);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(400, result.StatusCode);
    }

    //Unit tests for GetByReference, Update and Delete methods are skipped because will be similar with those from above
    //and they are out of the scope of the ingestion microservice as it's mentioned in the homework specifications
}
