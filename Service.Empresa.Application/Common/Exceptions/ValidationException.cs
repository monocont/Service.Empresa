namespace Service.Empresa.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public List<string> ValidationErrors { get; }

    public ValidationException(List<string> errors)
        : base("Se produjeron uno o más errores de validación.")
    {
        ValidationErrors = errors;
    }

    public ValidationException(string error)
        : base(error)
    {
        ValidationErrors = [error];
    }
}