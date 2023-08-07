using FluentResults;
using IncomeTaxCalculator.Domain.Entities;
using IncomeTaxCalculator.Domain.Errors;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Interfaces.Repositories;
using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Models.Responses;
using IncomeTaxCalculator.Domain.Services;
using Moq;
using NUnit.Framework;

namespace IncomeTaxCalculator.UnitTests.Domain.Services
{
    [TestFixture]
    public class SalaryTaxAppServiceTests
    {
        private Mock<ITaxCalculatorService> _mockTaxCalculatorService;
        private Mock<ITaxBandRepository> _mockTaxBandRepository;
        private SalaryTaxAppService _salaryTaxAppService;

        [SetUp]
        public void Setup()
        {
            _mockTaxCalculatorService = new Mock<ITaxCalculatorService>();
            _mockTaxBandRepository = new Mock<ITaxBandRepository>();
            _salaryTaxAppService = new SalaryTaxAppService(_mockTaxCalculatorService.Object, _mockTaxBandRepository.Object);
        }

        [Test]
        public async Task GetCalculatedSalaryAsync_Should_ReturnCalculatedSalaryResponse()
        {
            // Arrange
            decimal grossAnnualSalary = 40000;
            var taxBands = new List<TaxBand>
            {
                new TaxBand { LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
                new TaxBand { LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
                new TaxBand { LowerLimit = 20000, TaxRate = 40 }
            };

            _mockTaxBandRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(taxBands);

            var expectedResponse = new CalculatedSalaryResponse
            {
                GrossAnnualSalary = 40000,
                AnnualTaxPaid = 11000,
                NetAnnualSalary = 29000
            };

            _mockTaxCalculatorService
                .Setup(service => service.CalculateTax(grossAnnualSalary, taxBands))
                .Returns(Result.Ok(expectedResponse));

            // Act
            var result = await _salaryTaxAppService.GetCalculatedSalaryAsync(grossAnnualSalary);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task GetCalculatedSalaryAsync_Should_ReturnErrorResult_When_TaxBandRepositoryReturnsEmptyList()
        {
            // Arrange
            decimal grossAnnualSalary = 40000;
            var cancellationToken = CancellationToken.None;

            _mockTaxCalculatorService
                .Setup(service => service.CalculateTax(It.IsAny<decimal>(), It.IsAny<List<TaxBand>>()))
                .Returns(Result.Fail(new NotFoundError("")));
            
            // Act
            var result = await _salaryTaxAppService.GetCalculatedSalaryAsync(grossAnnualSalary, cancellationToken);

            // Assert
            Assert.That(result.IsFailed, Is.True);
            Assert.IsInstanceOf<NotFoundError>(result.Errors.First());
        }

        [Test]
        public async Task GetCalculatedSalaryAsync_Should_ReturnErrorResult_When_GrossAnnualSalaryIsNegative()
        {
            // Arrange
            decimal grossAnnualSalary = -10000;
            var cancellationToken = CancellationToken.None;

            _mockTaxCalculatorService
                .Setup(service => service.CalculateTax(It.IsAny<decimal>(), It.IsAny<List<TaxBand>>()))
                .Returns(Result.Fail(new ValidationError("")));

            // Act
            var result = await _salaryTaxAppService.GetCalculatedSalaryAsync(grossAnnualSalary, cancellationToken);

            // Assert
            Assert.That(result.IsFailed, Is.True);
            Assert.IsInstanceOf<ValidationError>(result.Errors.First());
        }
    }
}
