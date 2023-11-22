using Analysis.Application.Models;
using Analysis.Application.Services;
using Analysis.Domain.Entities;
using Analysis.Domain.RepositoryInterfaces;
using MockQueryable.Moq;
using Moq;

namespace Analysis.Tests.Unit.Services;

public class InvoiceServiceTests
{
    private Mock<IInvoiceRepository>? _invoiceRepositoryMock;
    private InvoiceService sut;


    private List<Invoice> _invoices = new List<Invoice>
    {
        new Invoice
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
        new Invoice
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

    public InvoiceServiceTests()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
  
        sut = new InvoiceService(_invoiceRepositoryMock.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new InvoiceService(null!));
    }

    [Fact]
    public async Task GetAllInvoicesAsync_WhenDataExists_ReturnsData()
    {
        // Arrange
        var expectedInvoiceCount = _invoices.Count;
        _invoiceRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync(_invoices);

        // Act
        var result = await sut.GetAllInvoicesAsync()!;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expectedInvoiceCount, result.Count);
    }

    [Fact]
    public async Task GetAllInvoicesAsync_WhenDataDoesNotExists_ReturnsNull()
    {
        // Arrange
        _invoiceRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync((List<Invoice>)null!);

        // Act
        var result = await sut.GetAllInvoicesAsync()!;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetInvoiceByReferenceAsync_WhenDataExists_ReturnsData()
    {
        // Arrange
        var expectedInvoice = _invoices.First();
        var expectedReference = expectedInvoice.Reference;
        _invoiceRepositoryMock!.Setup(x => x.GetByReferenceAsync(expectedReference)).ReturnsAsync(expectedInvoice);

        // Act
        var result = await sut.GetInvoiceByReferenceAsync(expectedReference)!;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedReference, result.Reference);
    }

    [Fact]
    public async Task GetSummaryInvoiceAsync_WhenDataExists_ReturnsData()
    {
        // Arrange
        var expectedStartDate = _invoices.First().IssueDate;
        var expectedEndDate = _invoices.First().IssueDate;
        var expectedIncludeClosedInvoices = true;
        var expectedIncludeOpenInvoices = false;
        var expectedAmount = _invoices.Sum(i => i.OpeningValue);
        var expectedSummaryInvoice = new SummaryInvoice
        {
            StartDate = expectedStartDate,
            EndDate = expectedEndDate,
            IncludeClosedInvoices = expectedIncludeClosedInvoices,
            IncludeOpenInvoices = expectedIncludeOpenInvoices,
            TotalAmount = expectedAmount,
            Invoices = _invoices
        };

        var expectedInvoices = _invoices.BuildMock();
        _invoiceRepositoryMock!.Setup(x => x.GetAllQueryable()).Returns(expectedInvoices);

        // Act
        var result = await sut.GetSummaryInvoiceAsync(expectedStartDate, expectedEndDate, expectedIncludeClosedInvoices, expectedIncludeOpenInvoices)!;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSummaryInvoice.Invoices.Count, result.Invoices.Count);
        Assert.Equal(expectedStartDate, result.StartDate);
        Assert.Equal(expectedEndDate, result.EndDate);
        Assert.Equal(expectedIncludeClosedInvoices, result.IncludeClosedInvoices);
        Assert.Equal(expectedIncludeOpenInvoices, result.IncludeOpenInvoices);
    }
}

