using FluentValidation;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorRuc;

public class ObtenerEmpresaPorRucValidator : AbstractValidator<ObtenerEmpresaPorRucQuery>
{
    public ObtenerEmpresaPorRucValidator()
    {
        RuleFor(x => x.Ruc)
            .NotEmpty().WithMessage("El RUC es obligatorio.")
            .Length(11).WithMessage("El RUC debe tener exactamente 11 caracteres.")
            .Matches("^[0-9]+$").WithMessage("El RUC debe contener solo dígitos.");
    }
}