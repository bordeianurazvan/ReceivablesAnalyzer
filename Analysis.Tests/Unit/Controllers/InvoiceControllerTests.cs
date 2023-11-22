using Analysis.Api.Controllers;
using Analysis.Application.Models;
using Analysis.Application.Services;
using Analysis.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Analysis.Tests.Unit.Controllers;

public class InvoiceControllerTests
{
    private Mock<ILogger<InvoiceController>>? _loggerMock;
    private Mock<IInvoiceService>? _invoiceServiceMock;

    private InvoiceController sut;

    private IList<Invoice> _invoices = new List<Invoice>
    {
        new Invoice
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
    public async Task GetAll_WhenDataExists_ReturnsOk()
    {
        // Arrange
        var expectedInvoicesCount = _invoices.Count;
        _invoiceServiceMock!.Setup(x => x.GetAllInvoicesAsync())!.ReturnsAsync(_invoices);

        // Act
        var response = await sut.Get();

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var invoices = Assert.IsType<List<Invoice>>(result.Value);
        Assert.Equal(expectedInvoicesCount, invoices.Count);
    }

    [Fact]
    public async Task GetAll_WhenDataDoesNotExists_ReturnsNotFound()
    {
        // Arrange
        _invoiceServiceMock!.Setup(x => x.GetAllInvoicesAsync())!.ReturnsAsync((List<Invoice>)null!);

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
        var expectedInvoice = _invoices.First();
        var expectedReference = expectedInvoice.Reference;
        _invoiceServiceMock!.Setup(x => x.GetInvoiceByReferenceAsync(expectedReference))!.ReturnsAsync(expectedInvoice);

        // Act
        var response = await sut.Get(expectedReference);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var invoice = Assert.IsType<Invoice>(result.Value);
        Assert.Equal(expectedReference, invoice.Reference);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetByReference_WhenInvalidReference_ReturnsBadRequest(string? reference)
    {
        // Arrange
        _invoiceServiceMock!.Setup(x => x.GetInvoiceByReferenceAsync(It.IsAny<string>()))!.ReturnsAsync(It.IsAny<Invoice>());

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
        _invoiceServiceMock!.Setup(x => x.GetSummaryInvoiceAsync(startDate, endDate, null, null))!.ReturnsAsync(It.IsAny<SummaryInvoice>);

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
        var expectedStartDate = _invoices.First().IssueDate;
        var expectedEndDate = _invoices.First().IssueDate;
        var expectedIncludeClosedInvoices = true;
        var expectedIncludeOpenInvoices = false;
        var expectedSummaryInvoice = new SummaryInvoice
        {
            StartDate = expectedStartDate,
            EndDate = expectedEndDate,
            IncludeClosedInvoices = expectedIncludeClosedInvoices,
            IncludeOpenInvoices = expectedIncludeOpenInvoices,
            Invoices = _invoices
        };
        _invoiceServiceMock!.Setup(x => x.GetSummaryInvoiceAsync(expectedStartDate, expectedEndDate, expectedIncludeClosedInvoices, expectedIncludeOpenInvoices))!.ReturnsAsync(expectedSummaryInvoice);

        // Act
        var response = await sut.Get(expectedStartDate, expectedEndDate, expectedIncludeClosedInvoices, expectedIncludeOpenInvoices);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var summaryInvoiceResult = Assert.IsType<SummaryInvoice>(result.Value);
        Assert.Equal(expectedStartDate, summaryInvoiceResult.StartDate);
        Assert.Equal(expectedEndDate, summaryInvoiceResult.EndDate);
        Assert.Equal(expectedIncludeClosedInvoices, summaryInvoiceResult.IncludeClosedInvoices);
        Assert.Equal(expectedIncludeOpenInvoices, summaryInvoiceResult.IncludeOpenInvoices);
        Assert.Equal(expectedSummaryInvoice.Invoices.Count, summaryInvoiceResult.Invoices.Count);
    }


    [Fact]
    public async Task GetSummary_WhenNoData_ReturnsNotFound()
    {
        // Arrange
        _invoiceServiceMock!.Setup(x => x.GetSummaryInvoiceAsync(null, null, null, null))!.ReturnsAsync((SummaryInvoice) null!);

        // Act
        var response = await sut.Get(null, null, null, null);

        // Assert
        Assert.NotNull(response);

        var result = Assert.IsType<NotFoundResult>(response);
        Assert.Equal(404, result.StatusCode);
    }
}
