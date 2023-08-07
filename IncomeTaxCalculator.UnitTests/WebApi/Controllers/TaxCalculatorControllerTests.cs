using FluentResults;
using IncomeTaxCalculator.Domain.Errors;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Interfaces.Services;
using IncomeTaxCalculator.Domain.Models.Requests;
using IncomeTaxCalculator.Domain.Models.Responses;
using IncomeTaxCalculator.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace IncomeTaxCalculator.UnitTests.WebApi.Controllers
{
    public class TaxCalculatorControllerTests
    {
        private TaxCalculatorController _controller;
        private Mock<ISalaryTaxAppService> _salaryTaxAppServiceMock;
        private Mock<ILogger<TaxCalculatorController>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _salaryTaxAppServiceMock = new ();
            _loggerMock = new ();

            _controller = new TaxCalculatorController(_loggerMock.Object, _salaryTaxAppServiceMock.Object);
        }

        [Test]
        public async Task CalculatedSalary_Should_Return_Ok_Result_With_Calculated_Salary()
        {
            // Arrange
            var request = new CalculateSalaryTaxRequest { GrossAnnualSalary = 40000m };
            var calculatedSalaryResponse = new CalculatedSalaryResponse
            {
                GrossAnnualSalary = 40000m,
                NetAnnualSalary = 29000m,
                AnnualTaxPaid = 11000m
            };
            var expectedResult = Result.Ok(calculatedSalaryResponse);
            _salaryTaxAppServiceMock
                .Setup(x => x.GetCalculatedSalaryAsync(request.GrossAnnualSalary, CancellationToken.None))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CalculatedSalary(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task CalculatedSalary_Should_Return_BadRequest_When_Validation_Error_Occurs()
        {
            // Arrange
            var request = new CalculateSalaryTaxRequest { GrossAnnualSalary = -1000m };
            var errorMessage = "Salary cannot be negative";
            var errorResult = Result.Fail(new ValidationError(errorMessage));
            _salaryTaxAppServiceMock.Setup(x => x.GetCalculatedSalaryAsync(request.GrossAnnualSalary, CancellationToken.None)).ReturnsAsync(errorResult);

            // Act
            var result = await _controller.CalculatedSalary(request, CancellationToken.None);

            // Assert
            _loggerMock.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(errorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ), Times.Once
            );
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task CalculatedSalary_Should_Return_NotFound_When_NotFound_Error_Occurs()
        {
            // Arrange
            var request = new CalculateSalaryTaxRequest { GrossAnnualSalary = 5000m };
            var errorMessage = "Employee not found";
            var errorResult = Result.Fail(new NotFoundError(errorMessage));
            _salaryTaxAppServiceMock.Setup(x => x.GetCalculatedSalaryAsync(request.GrossAnnualSalary, CancellationToken.None)).ReturnsAsync(errorResult);

            // Act
            var result = await _controller.CalculatedSalary(request, CancellationToken.None);

            // Assert

            _loggerMock.Verify(l =>
                l.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(errorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ), Times.Once
            );
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
    }
}
