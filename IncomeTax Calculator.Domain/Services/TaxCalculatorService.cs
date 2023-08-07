using FluentResults;
using IncomeTaxCalculator.Domain.Entities;
using IncomeTaxCalculator.Domain.Errors;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Models.Responses;

namespace IncomeTaxCalculator.Domain.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        public Result<CalculatedSalaryResponse> CalculateTax(decimal grossAnnualSalary, List<TaxBand> taxBands)
        {
            if (grossAnnualSalary < 0)
            {
                return Result.Fail(new ValidationError("Salary cannot be negative"));
            }
            if (taxBands is null || taxBands.Count <= 0)
            {
                return Result.Fail(new NotFoundError("TaxBand cannot be empty"));
            }

            decimal annualTaxPaid = 0;
            decimal remainingSalary = grossAnnualSalary;

            foreach (var band in taxBands)
            {
                decimal taxableAmountInBand = Math.Min(remainingSalary, band.UpperLimit - band.LowerLimit);
                decimal taxInBand = taxableAmountInBand * band.TaxRate / 100;
                annualTaxPaid += taxInBand;
                remainingSalary -= taxableAmountInBand;
            }

            var salary = new CalculatedSalaryResponse
            {
                GrossAnnualSalary = grossAnnualSalary,
                AnnualTaxPaid = annualTaxPaid,
                NetAnnualSalary = grossAnnualSalary - annualTaxPaid,
            };

            return salary;
        }
    }
}
