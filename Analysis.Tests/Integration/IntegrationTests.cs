using Analysis.Api;
using Analysis.Application.Models;
using Analysis.Domain.Entities;
using System.Net.Http.Json;

namespace Analysis.Tests.Integration;
public class IntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestingWebAppFactory<Program> _factory;

    public IntegrationTests(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_Invoice_EndpointReturnSuccessAndProperData()
    {
        // Act
        var response = await _client.GetAsync("/invoice");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<List<Invoice>>();
        Assert.NotEmpty(jsonResponse!);
    }

    [Fact]
    public async Task GetByReference_Invoice_EndpointReturnSuccessAndProperData()
    {
        // Arrange
        var seedInvoiceReference = _factory.seedInvoice.Reference;

        // Act
        var response = await _client.GetAsync($"/invoice/{seedInvoiceReference}");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<Invoice>();
        Assert.NotNull(jsonResponse!);
        Assert.Equal(seedInvoiceReference, jsonResponse.Reference);
    }

    [Fact]
    public async Task GetSummary_Invoice_EndpointReturnSuccessAndProperData()
    {
        // Arrange
        var seedInvoice = _factory.seedInvoice;
        var expectedSummaryInvoice = new SummaryInvoice
        {
            StartDate = null,
            EndDate = null,
            IncludeClosedInvoices = null,
            IncludeOpenInvoices = null,
            Invoices = new List<Invoice> { seedInvoice },
            TotalAmount = seedInvoice.OpeningValue
        };

        // Act
        var response = await _client.GetAsync($"/invoice/summary");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<SummaryInvoice>();
        Assert.NotNull(jsonResponse!);
        Assert.Equal(expectedSummaryInvoice.TotalAmount, jsonResponse.TotalAmount);
        Assert.Equal(expectedSummaryInvoice.Invoices.Count, jsonResponse.Invoices.Count);
        Assert.Equal(expectedSummaryInvoice.StartDate, jsonResponse.StartDate);
        Assert.Equal(expectedSummaryInvoice.EndDate, jsonResponse.EndDate);
        Assert.Equal(expectedSummaryInvoice.IncludeOpenInvoices, jsonResponse.IncludeOpenInvoices);
        Assert.Equal(expectedSummaryInvoice.IncludeClosedInvoices, jsonResponse.IncludeClosedInvoices);
    }

    [Fact]
    public async Task GetAll_CreditNote_EndpointReturnSuccessAndProperData()
    {
        // Act
        var response = await _client.GetAsync("/creditNote");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<List<CreditNote>>();
        Assert.NotEmpty(jsonResponse!);
    }

    [Fact]
    public async Task GetByReference_CreditNote_EndpointReturnSuccessAndProperData()
    {
        // Arrange
        var seedCreditNoteReference = _factory.seedCreditNote.Reference;

        // Act
        var response = await _client.GetAsync($"/creditNote/{seedCreditNoteReference}");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<CreditNote>();
        Assert.NotNull(jsonResponse!);
        Assert.Equal(seedCreditNoteReference, jsonResponse.Reference);
    }

    [Fact]
    public async Task GetSummary_CreditNote_EndpointReturnSuccessAndProperData()
    {
        // Arrange
        var seedCreditNote = _factory.seedCreditNote;
        var expectedSummaryCreditNote = new SummaryCreditNote
        {
            StartDate = null,
            EndDate = null,
            IncludeClosedCreditNotes = null,
            IncludeOpenCreditNotes = null,
            CreditNotes = new List<CreditNote> { seedCreditNote },
            TotalAmount = seedCreditNote.OpeningValue
        };

        // Act
        var response = await _client.GetAsync($"/creditNote/summary");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<SummaryCreditNote>();
        Assert.NotNull(jsonResponse!);
        Assert.Equal(expectedSummaryCreditNote.TotalAmount, jsonResponse.TotalAmount);
        Assert.Equal(expectedSummaryCreditNote.CreditNotes.Count, jsonResponse.CreditNotes.Count);
        Assert.Equal(expectedSummaryCreditNote.StartDate, jsonResponse.StartDate);
        Assert.Equal(expectedSummaryCreditNote.EndDate, jsonResponse.EndDate);
        Assert.Equal(expectedSummaryCreditNote.IncludeOpenCreditNotes, jsonResponse.IncludeOpenCreditNotes);
        Assert.Equal(expectedSummaryCreditNote.IncludeClosedCreditNotes, jsonResponse.IncludeClosedCreditNotes);
    }
}
