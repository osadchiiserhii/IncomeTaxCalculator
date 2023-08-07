using FluentResults;
using IncomeTaxCalculator.Domain.Models.Responses;

namespace IncomeTaxCalculator.Domain.Interfaces.Services
{
    public interface ISalaryTaxAppService
    {
        Task<Result<CalculatedSalaryResponse>> GetCalculatedSalaryAsync(decimal grossAnnualSalary, CancellationToken cancellationToken);
    }
}
