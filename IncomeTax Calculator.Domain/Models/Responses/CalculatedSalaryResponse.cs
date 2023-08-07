namespace IncomeTaxCalculator.Domain.Models.Responses
{
    public class CalculatedSalaryResponse
    {
        private decimal _grossAnnualSalary;
        public decimal GrossAnnualSalary
        {
            get => _grossAnnualSalary;
            set => _grossAnnualSalary = Math.Round(value, 2);
        }

        private decimal _netAnnualSalary;
        public decimal NetAnnualSalary
        {
            get => _netAnnualSalary;
            set => _netAnnualSalary = Math.Round(value, 2);
        }

        private decimal _annualTaxPaid;
        public decimal AnnualTaxPaid
        {
            get => _annualTaxPaid;
            set => _annualTaxPaid = Math.Round(value, 2);
        }

        public decimal GrossMonthlySalary => Math.Round(GrossAnnualSalary / 12, 2);
        public decimal NetMonthlySalary => Math.Round(NetAnnualSalary / 12, 2);
        public decimal MonthlyTaxPaid => Math.Round(AnnualTaxPaid / 12, 2);
    }
}
