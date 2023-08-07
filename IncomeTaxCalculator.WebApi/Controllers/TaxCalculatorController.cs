using FluentResults;
using IncomeTaxCalculator.Domain.Errors;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Models.Requests;
using IncomeTaxCalculator.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly ILogger<TaxCalculatorController> _logger;
        private readonly ISalaryTaxAppService _salaryTaxAppService;

        public TaxCalculatorController(
            ILogger<TaxCalculatorController> logger,
            ISalaryTaxAppService salaryTaxAppService

            )
        {
            _logger = logger;
            _salaryTaxAppService = salaryTaxAppService;
        }

        [HttpPost]
        public async Task<ActionResult<CalculatedSalaryResponse>> CalculatedSalary([FromBody]CalculateSalaryTaxRequest request, CancellationToken cancellationToken)
        {
            var result = await _salaryTaxAppService.GetCalculatedSalaryAsync(request.GrossAnnualSalary, cancellationToken);
            if (result.HasError<ValidationError>())
            {
                var errorMessage = GetErrorMessage(result);
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
            if (result.HasError<NotFoundError>())
            {
                var errorMessage = GetErrorMessage(result);
                _logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            return Ok(result.Value);
        }

        private string GetErrorMessage(Result<CalculatedSalaryResponse> result) =>  string.Join("\n", result.Errors.Select(i => i.Message));
    }
}
