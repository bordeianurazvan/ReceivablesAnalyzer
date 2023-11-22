using Ingestion.Api;
using Ingestion.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Z.EntityFramework.Extensions;

namespace Ingestion.Tests.Integration;

public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<DatabaseContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryReceivablesTest");
                EntityFrameworkManager.ContextFactory = container => new DatabaseContext((DbContextOptions<DatabaseContext>)options.Options);
            });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                   
                    appContext.Invoices.Add(new Domain.Entities.Invoice
                    {
                        Reference = Guid.NewGuid().ToString(),
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
                    });
                    appContext.CreditNotes.Add(new Domain.Entities.CreditNote
                    {
                        Reference = Guid.NewGuid().ToString(),
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
                    });
                    appContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        });
    }
}
