using IncomeTaxCalculator.Domain.Common;
using IncomeTaxCalculator.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IncomeTaxCalculator.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }
    }
}
