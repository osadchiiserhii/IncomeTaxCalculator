namespace IncomeTaxCalculator.Domain.Models.Requests
{
    public record CalculateSalaryTaxRequest
    {
        public decimal GrossAnnualSalary { get; set; }
    }
}
