using Ingestion.Api.Controllers;
using Ingestion.Application.Models;
using Ingestion.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ingestion.Tests.Unit.Controllers;

public class InvoiceControllerTests
{
    private Mock<ILogger<InvoiceController>>? _loggerMock;
    private Mock<IInvoiceService>? _invoiceServiceMock;

    private InvoiceController sut;

    private IList<InvoiceDto> _invoices = new List<InvoiceDto>
    {
        new InvoiceDto
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
        new InvoiceDto
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

    public InvoiceControllerTests()
    {
        _loggerMock = new Mock<ILogger<InvoiceController>>();
        _invoiceServiceMock = new Mock<IInvoiceService>();

        sut = new InvoiceController(_loggerMock.Object, _invoiceServiceMock.Object);
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new InvoiceController(null!, _invoiceServiceMock!.Object));
    }

    [Fact]
    public void Constructor_WithNullService_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new InvoiceController(_loggerMock!.Object, null!));
    }

    [Fact]
    public async Task Get_WhenDataExists_ReturnsOk()
    {
        // Arrange
        var expectedInvoicesCount = _invoices.Count;
        _invoiceServiceMock!.Setup(x => x.GetAllAsync())!.ReturnsAsync(_invoices);

        // Act
        var response = await sut.Get();

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var invoiceDtos = Assert.IsType<List<InvoiceDto>>(result.Value);
        Assert.Equal(expectedInvoicesCount, invoiceDtos.Count);
    }

    [Fact]
    public async Task Get_WhenDataDoesNotExists_ReturnsNotFound()
    {
        // Arrange
        _invoiceServiceMock!.Setup(x => x.GetAllAsync())!.ReturnsAsync((List<InvoiceDto>)null!);

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
        var expectedInvoice = new InvoiceDto
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
        var expectedInvoices = new List<InvoiceDto>
        {
            expectedInvoice
        };

        _invoiceServiceMock!.Setup(x => x.InsertAsync(expectedInvoices)).ReturnsAsync(expectedInvoices);

        // Act
        var response = await sut.Post(expectedInvoices);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var invoiceDtos = Assert.IsType<List<InvoiceDto>>(result.Value);
        var invoiceDto = invoiceDtos.FirstOrDefault(x => x.Reference == expectedReference);
        Assert.NotNull(invoiceDto);
        Assert.Equal(expectedReference, invoiceDto.Reference);
    }

    [Fact]
    public async Task Post_WhenInvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var invalidInput = (List<InvoiceDto>)null!;
        _invoiceServiceMock!.Setup(x => x.InsertAsync(It.IsAny<List<InvoiceDto>>())).ReturnsAsync((List<InvoiceDto>)null!);

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
