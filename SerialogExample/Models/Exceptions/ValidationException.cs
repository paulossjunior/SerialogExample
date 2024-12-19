namespace SerialogExample.Models.Exceptions;

public class ValidationException: Exception
{
    public ValidationException(string message) : base(message) { }
}