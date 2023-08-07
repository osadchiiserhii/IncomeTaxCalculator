using IncomeTaxCalculator.Domain.Common;

namespace IncomeTaxCalculator.Domain.Entities
{
    public sealed class TaxBand : BaseEntity
    {
        public string BandName { get; set; } = null!;
        public decimal LowerLimit { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal TaxRate { get; set; }
    }
}
