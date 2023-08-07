using FluentResults;
using IncomeTaxCalculator.Domain.Errors;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Interfaces.Repositories;
using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Models.Responses;

namespace IncomeTaxCalculator.Domain.Services
{
    public class SalaryTaxAppService: ISalaryTaxAppService
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ITaxBandRepository _taxBandRepository;

        public SalaryTaxAppService(
            ITaxCalculatorService taxCalculatorService,
            ITaxBandRepository taxBandRepository
            )
        {
            _taxCalculatorService = taxCalculatorService;
            _taxBandRepository = taxBandRepository;
        }

        public async Task<Result<CalculatedSalaryResponse>> GetCalculatedSalaryAsync(decimal grossAnnualSalary, CancellationToken cancellationToken = default)
        {
            var taxBands = await _taxBandRepository.GetAllAsync(cancellationToken);
            return _taxCalculatorService.CalculateTax(grossAnnualSalary, taxBands);
        }

    }
}
