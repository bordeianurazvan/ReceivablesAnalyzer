using Ingestion.Domain.Entities;
using Ingestion.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Ingestion.Infrastructure.Repositories;

public class CreditNoteRepository : ICreditNoteRepository
{
    private readonly DatabaseContext _dbContext;

    public CreditNoteRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IList<CreditNote>> GetAllAsync()
    {
        return await _dbContext.CreditNotes.ToListAsync();
    }

    public async Task<CreditNote?> GetByReferenceAsync(string reference)
    {
        return await _dbContext.CreditNotes.FirstOrDefaultAsync(e => e.Reference == reference);
    }

    public async Task<CreditNote> InsertAsync(CreditNote entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<IList<CreditNote>> InsertBulkAsync(IList<CreditNote> entities)
    {
        await _dbContext.BulkInsertAsync(entities, operation => operation.IncludeGraph = true);
        await _dbContext.SaveChangesAsync();

        return entities;
    }

    public async Task<CreditNote> UpdateAsync(CreditNote entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(string reference)
    {
        var entity = _dbContext.CreditNotes.FirstOrDefault(e => e.Reference == reference);
        if (entity == null)
            return await _dbContext.SaveChangesAsync() > 0;

        _dbContext.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
