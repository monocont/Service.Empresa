using FluentValidation;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorId;

public class ObtenerEmpresaPorIdValidator : AbstractValidator<ObtenerEmpresaPorIdQuery>
{
    public ObtenerEmpresaPorIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id de la empresa es obligatorio.");
    }
}