namespace IncomeTaxCalculator.Domain.Errors
{
    public class ValidationError : FluentResults.Error
    {
        public ValidationError(string message)
        {
            Message = message;
        }
    }
}
