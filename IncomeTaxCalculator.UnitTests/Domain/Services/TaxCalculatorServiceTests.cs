using IncomeTaxCalculator.Domain.Entities;
using IncomeTaxCalculator.Domain.Errors;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Services;
using NUnit.Framework;

namespace IncomeTaxCalculator.UnitTests.Domain.Services
{
    [TestFixture]
    public class TaxCalculatorServiceTests
    {
        private TaxCalculatorService _taxCalculator;

        [SetUp]
        public void SetUp()
        {
            _taxCalculator = new TaxCalculatorService();
        }

        [Test]
        public void CalculateTax_Should_Return_Correct_Salary_With_Tax_Bands()
        {
            // Arrange
            var grossAnnualSalary = 30000m;
            var taxBands = new List<TaxBand>
            {
                new TaxBand { LowerLimit = 0m, UpperLimit = 5000m, TaxRate = 0m },
                new TaxBand { LowerLimit = 5000m, UpperLimit = 20000m, TaxRate = 20m },
                new TaxBand { LowerLimit = 20000m, UpperLimit = decimal.MaxValue, TaxRate = 40m }
            };

            // Act
            var result = _taxCalculator.CalculateTax(grossAnnualSalary, taxBands);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.GrossAnnualSalary, Is.EqualTo(30000m));
            Assert.That(result.Value.AnnualTaxPaid, Is.EqualTo(7000m));
            Assert.That(result.Value.NetAnnualSalary, Is.EqualTo(23000m));
        }

        [Test]
        public void CalculateTax_Should_Return_ValidationError_For_Negative_Salary()
        {
            // Arrange
            var grossAnnualSalary = -5000m;
            var taxBands = new List<TaxBand>();
            // Act
            var result = _taxCalculator.CalculateTax(grossAnnualSalary, taxBands);

            //Assert
            Assert.That(result.IsFailed, Is.True);
            Assert.IsInstanceOf<ValidationError>(result.Errors.First());
        }

        [Test]
        public void CalculateTax_Should_Return_NotFoundError_For_Null_Or_Empty_TaxBands()
        {
            // Arrange
            var grossAnnualSalary = 40000m;
            var taxBands = new List<TaxBand>();

            // Act
            var result = _taxCalculator.CalculateTax(grossAnnualSalary, taxBands);

            //Assert
            Assert.That(result.IsFailed, Is.True);
            Assert.IsInstanceOf<NotFoundError>(result.Errors.First());
        }
    }
}