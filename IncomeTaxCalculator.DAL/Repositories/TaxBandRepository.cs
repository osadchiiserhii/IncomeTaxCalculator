using IncomeTaxCalculator.Domain.Entities;
using IncomeTaxCalculator.Domain.Interfaces.Repositories;

namespace IncomeTaxCalculator.DAL.Repositories
{
    public class TaxBandRepository : BaseRepository<TaxBand>, ITaxBandRepository
    {
        public TaxBandRepository(DataContext context) : base(context)
        {
        }
    }
}
