using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IncomeTaxCalculator.Domain.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();
            services.AddScoped<ISalaryTaxAppService, SalaryTaxAppService>();
        }
    }
}
