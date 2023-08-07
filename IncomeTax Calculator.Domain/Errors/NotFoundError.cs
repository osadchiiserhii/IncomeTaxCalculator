namespace IncomeTaxCalculator.Domain.Exceptions
{
    public class NotFoundError : FluentResults.Error
    {
        public NotFoundError(string message) 
        {
            Message = message;
        }
    }
}
