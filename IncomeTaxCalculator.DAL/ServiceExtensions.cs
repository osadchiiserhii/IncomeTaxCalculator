using IncomeTaxCalculator.DAL.Repositories;
using IncomeTaxCalculator.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace IncomeTaxCalculator.DAL
{
    public static class ServiceExtensions
    {
        public static void ConfigureDAL(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");
            services.AddDbContext<DataContext>(option => option.UseNpgsql(connectionString));

            services.AddScoped<ITaxBandRepository, TaxBandRepository>();
        }
    }
}
