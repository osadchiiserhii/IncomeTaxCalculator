using FluentResults;
using IncomeTaxCalculator.Domain.Entities;
using IncomeTaxCalculator.Domain.Models.Responses;

namespace IncomeTaxCalculator.Domain.Interfaces.Services
{
    public interface ITaxCalculatorService
    {
        Result<CalculatedSalaryResponse> CalculateTax(decimal grossAnnualSalary, List<TaxBand> taxBands);
    }
}
