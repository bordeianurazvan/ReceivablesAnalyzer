using Ingestion.Domain.Entities;
using static Ingestion.Domain.RepositoryInterfaces.IGenericRepository;

namespace Ingestion.Domain.RepositoryInterfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
    }
}
