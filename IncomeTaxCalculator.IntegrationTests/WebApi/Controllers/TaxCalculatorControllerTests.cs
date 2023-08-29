using IncomeTaxCalculator.Domain.Entities;
using IncomeTaxCalculator.Domain.Interfaces.Repositories;
using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Models.Requests;
using IncomeTaxCalculator.Domain.Models.Responses;
using IncomeTaxCalculator.Domain.Services;
using IncomeTaxCalculator.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace IncomeTaxCalculator.IntegrationTests.WebApi.Controllers
{
    public class TaxCalculatorControllerTests
    {
        private Mock<ILogger<TaxCalculatorController>> _loggerMock;
        private Mock<ITaxBandRepository> _mockTaxBandRepository;

        private TaxCalculatorController _controller;
        private ISalaryTaxAppService _salaryTaxAppService;
        private ITaxCalculatorService _taxCalculatorService;

        [SetUp]
        public void SetUp()
        {
            _taxCalculatorService = new TaxCalculatorService();
            _mockTaxBandRepository = new Mock<ITaxBandRepository>();
            _salaryTaxAppService = new SalaryTaxAppService(_taxCalculatorService, _mockTaxBandRepository.Object);
            _loggerMock = new();

            _controller = new TaxCalculatorController(_loggerMock.Object, _salaryTaxAppService);
        }

        [Test]
        public async Task CalculatedSalary_Should_Return_Ok_Result_With_Calculated_Salary()
        {
            // Arrange
            SetupMockTaxBandRepository();
            var expectedResult = new CalculatedSalaryResponse
            {
                GrossAnnualSalary = 40000m,
                NetAnnualSalary = 29000m,
                AnnualTaxPaid = 11000m
            };

            var request = new CalculateSalaryTaxRequest { GrossAnnualSalary = 40000m };

            // Act
            var actionResult = await _controller.CalculatedSalary(request, CancellationToken.None);

            // Assert
            var result = actionResult.Result as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task CalculatedSalary_Should_Return_BadRequest_When_Validation_Error_Occurs()
        {
            // Arrange
            SetupMockTaxBandRepository();
            var request = new CalculateSalaryTaxRequest { GrossAnnualSalary = -1000m };
            var expectedErrorMessage = "Salary cannot be negative";

            // Act
            var actionResult = await _controller.CalculatedSalary(request, CancellationToken.None);

            // Assert
            _loggerMock.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(expectedErrorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ), Times.Once
            );

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
            var result = actionResult.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(expectedErrorMessage));
        }

        [Test]
        public async Task CalculatedSalary_Should_Return_NotFound_When_NotFound_Error_Occurs()
        {
            // Arrange
            var request = new CalculateSalaryTaxRequest { GrossAnnualSalary = 5000m };
            var expectedErrorMessage = "TaxBand cannot be empty";

            // Act
            var actionResult = await _controller.CalculatedSalary(request, CancellationToken.None);

            // Assert
            _loggerMock.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(expectedErrorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ), Times.Once
            );

            Assert.IsInstanceOf<NotFoundObjectResult>(actionResult.Result);
            var result = actionResult.Result as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(expectedErrorMessage));
        }

        private void SetupMockTaxBandRepository()
        {
            var taxBands = new List<TaxBand>
            {
                new TaxBand { LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
                new TaxBand { LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
                new TaxBand { LowerLimit = 20000, UpperLimit = decimal.MaxValue, TaxRate = 40 }
            };

            _mockTaxBandRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(taxBands);
        }
    }
}
