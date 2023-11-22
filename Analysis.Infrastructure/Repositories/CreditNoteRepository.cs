using Analysis.Domain.Entities;
using Analysis.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Analysis.Infrastructure.Repositories;

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

    public IQueryable<CreditNote> GetAllQueryable()
    {
        return _dbContext.CreditNotes.AsQueryable();
    }
}
