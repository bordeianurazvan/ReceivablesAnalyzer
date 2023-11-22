using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Ingestion.Infrastructure.Repositories;

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

    public async Task<Invoice> InsertAsync(Invoice entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<IList<Invoice>> InsertBulkAsync(IList<Invoice> entities)
    {
        await _dbContext.BulkInsertAsync(entities, operation => operation.IncludeGraph = true);
        await _dbContext.SaveChangesAsync();

        return entities;
    }

    public async Task<Invoice> UpdateAsync(Invoice entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(string reference)
    {
        var entity = _dbContext.Invoices.FirstOrDefault(e => e.Reference == reference);
        if (entity == null)
            return false;

        _dbContext.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
