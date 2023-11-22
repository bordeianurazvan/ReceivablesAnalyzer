using AutoMapper;
using Ingestion.Application.Models;
using Ingestion.Application.Services;
using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ingestion.Tests.Unit.Services;

public class InvoiceServiceTests
{
    private Mock<IInvoiceRepository>? _invoiceRepositoryMock;
    private Mock<IMapper>? _mapperMock;
    private Mock<ILogger<InvoiceService>>? _loggerMock;

    private InvoiceService sut;


    private List<InvoiceDto> _invoiceDtos = new List<InvoiceDto>
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
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<InvoiceService>>();

        sut = new InvoiceService(_invoiceRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Constructor_WithNullRepository_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new InvoiceService(null!, _mapperMock!.Object, _loggerMock!.Object));
    }

    [Fact]
    public void Constructor_WithNullMapper_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new InvoiceService(_invoiceRepositoryMock!.Object, null!, _loggerMock!.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsException()
    {
        // Arrange
        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new InvoiceService(_invoiceRepositoryMock!.Object, _mapperMock!.Object, null!));
    }

    [Fact]
    public async Task GetAllAsync_WhenDataExists_ReturnsMappedData()
    {
        // Arrange
        var expectedInvoiceCount = _invoices.Count;
        _invoiceRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync(_invoices);
        _mapperMock!.Setup(x => x.Map<IList<InvoiceDto>>(_invoices)).Returns(_invoiceDtos);

        // Act
        var result = await sut.GetAllAsync()!;

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(expectedInvoiceCount, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenDataDoesNotExists_ReturnsNull()
    {
        // Arrange
        _invoiceRepositoryMock!.Setup(x => x.GetAllAsync()).ReturnsAsync((List<Invoice>)null!);

        // Act
        var result = await sut.GetAllAsync()!;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task InsertAsync_WhenValidInput_MapAndReturnInsertedData()
    {
        // Arrange
        var invoiceDto = _invoiceDtos.First();
        var invoiceDtos = new List<InvoiceDto>
        {
            invoiceDto
        };

        var expectedMappedInvoice = _invoices.First();
        IList<Invoice> expectedMappedInvoices = new List<Invoice>
        {
            expectedMappedInvoice
        };
        _invoiceRepositoryMock!.Setup(x => x.InsertAsync(expectedMappedInvoice))!.ReturnsAsync(expectedMappedInvoice);
        _mapperMock!.Setup(x => x.Map<IList<Invoice>>(invoiceDtos)).Returns(expectedMappedInvoices);
        _mapperMock!.Setup(x => x.Map<IList<InvoiceDto>>(expectedMappedInvoices)).Returns(invoiceDtos);

        // Act
        var result = await sut.InsertAsync(invoiceDtos);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result!);

        var dto = result.FirstOrDefault();
        Assert.Equal(expectedMappedInvoice.Reference, dto?.Reference);
    }

    [Fact]
    public async Task InsertAsync_WhenBulkInsertInvalidInput_ReturnsOnlyValidData()
    {
        // Arrange
        var validInvoiceDto = _invoiceDtos.First();
        var invalidInvoiceDto = new InvoiceDto
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
        var invoiceDtos = new List<InvoiceDto>
        {
            validInvoiceDto,
            invalidInvoiceDto
        };

        Invoice expectedMappedValidInvoice = _invoices.First();
        Invoice expectedMappedInvalidInvoice = new Invoice
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
        IList<Invoice> mappedInvoices = new List<Invoice>
        {
            expectedMappedValidInvoice,
            expectedMappedInvalidInvoice
        };

        IList<Invoice> mappedValidInvoices = new List<Invoice> { expectedMappedValidInvoice };

        _invoiceRepositoryMock!.Setup(x => x.InsertBulkAsync(mappedValidInvoices))!.ReturnsAsync(mappedValidInvoices);
        _mapperMock!.Setup(x => x.Map<IList<Invoice>>(invoiceDtos)).Returns(mappedInvoices);
        _mapperMock!.Setup(x => x.Map<IList<InvoiceDto>>(mappedValidInvoices)).Returns(new List<InvoiceDto> { validInvoiceDto });

        // Act
        var result = await sut.InsertAsync(invoiceDtos);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result!);
        Assert.Equal(mappedValidInvoices.Count, result.Count);

        var dto = result.FirstOrDefault();
        Assert.Equal(expectedMappedValidInvoice.Reference, dto?.Reference);
    }

    //Unit tests for GetByReference, Update and Delete methods are skipped because will be similar with those from above
    //and they are out of the scope of the ingestion microservice as it's mentioned in the homework specifications
}
