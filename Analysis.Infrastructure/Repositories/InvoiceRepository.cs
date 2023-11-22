using Analysis.Domain.Entities;
using Analysis.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Analysis.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly DatabaseContext _dbContext;

    public InvoiceRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IList<Invoice>> GetAllAsync()
    {
        return await _dbContext.Invoices.ToListAsync();
    }

    public async Task<Invoice?> GetByReferenceAsync(string reference)
    {
        return await _dbContext.Invoices.FirstOrDefaultAsync(e => e.Reference == reference);
    }

    public IQueryable<Invoice> GetAllQueryable()
    {
        return _dbContext.Invoices.AsQueryable();
    }
}
