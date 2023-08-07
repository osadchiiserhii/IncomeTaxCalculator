using IncomeTaxCalculator.Domain.Common;

namespace IncomeTaxCalculator.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    }
}
