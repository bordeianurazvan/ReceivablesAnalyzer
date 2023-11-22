using Ingestion.Api;
using Ingestion.Application.Models;
using System.Net.Http.Json;
using System.Text;

namespace Ingestion.Tests.Integration;

public class BaseIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public BaseIntegrationTests(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Invoice_EndpointReturnSuccessAndProperData()
    {
        // Act
        var response = await _client.GetAsync("/invoice");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<List<InvoiceDto>>();
        Assert.NotEmpty(jsonResponse!);
    }

    [Fact]
    public async Task Post_Invoice_EndpointReturnSuccessAndProperData()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/invoice");
        var expectedReference = Guid.NewGuid().ToString();
        var invoiceDto = new InvoiceDto
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
        var invoiceDtos = new List<InvoiceDto>
        {
            invoiceDto
        };
        postRequest.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(invoiceDtos), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(postRequest);

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<List<InvoiceDto>>();
        Assert.NotNull(jsonResponse);
        Assert.Equal(invoiceDtos.Count, jsonResponse!.Count);


        var getAllInvoicesResponse = await _client.GetAsync("/invoice");
        getAllInvoicesResponse.EnsureSuccessStatusCode();

        var allInvoices = await getAllInvoicesResponse.Content.ReadFromJsonAsync<List<InvoiceDto>>();
        Assert.NotEmpty(allInvoices!);
        Assert.True(allInvoices!.Count > 1);
    }

    [Fact]
    public async Task Get_CreditNote_EndpointReturnSuccessAndProperData()
    {
        // Act
        var response = await _client.GetAsync("/invoice");

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<List<CreditNoteDto>>();
        Assert.NotEmpty(jsonResponse!);
    }

    [Fact]
    public async Task Post_CreditNote_EndpointReturnSuccessAndProperData()
    {
        // Arrange
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/creditNote");
        var expectedReference = Guid.NewGuid().ToString();
        var creditNoteDto = new CreditNoteDto
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
        var creditNoteDtos = new List<CreditNoteDto>
        {
            creditNoteDto
        };
        postRequest.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(creditNoteDtos), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(postRequest);

        // Assert
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<List<CreditNoteDto>>();
        Assert.NotNull(jsonResponse);
        Assert.Equal(creditNoteDtos.Count, jsonResponse!.Count);

        var getAllCreditNotesResponse = await _client.GetAsync("/creditNote");
        getAllCreditNotesResponse.EnsureSuccessStatusCode();

        var allInvoices = await getAllCreditNotesResponse.Content.ReadFromJsonAsync<List<CreditNoteDto>>();
        Assert.NotEmpty(allInvoices!);
        Assert.True(allInvoices!.Count > 1);
    }
}
