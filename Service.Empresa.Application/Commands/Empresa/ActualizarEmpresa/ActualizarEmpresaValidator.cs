using FluentValidation;

namespace Service.Empresa.Application.Commands.Empresa.ActualizarEmpresa;

public class ActualizarEmpresaValidator : AbstractValidator<ActualizarEmpresaCommand>
{
    public ActualizarEmpresaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id de la empresa es obligatorio.");

        RuleFor(x => x.Ruc)
            .NotEmpty().WithMessage("El RUC es obligatorio.")
            .Length(11).WithMessage("El RUC debe tener exactamente 11 caracteres.")
            .Matches("^[0-9]+$").WithMessage("El RUC debe contener solo dígitos.")
            .Must(ruc => ruc.StartsWith("10") || ruc.StartsWith("20"))
                .WithMessage("El RUC debe comenzar con 10 o 20.");

        RuleFor(x => x.RazonSocial)
            .NotEmpty().WithMessage("La razón social es obligatoria.")
            .MaximumLength(255).WithMessage("La razón social no puede exceder los 255 caracteres.");

        RuleFor(x => x.NombreComercial)
            .MaximumLength(255).WithMessage("El nombre comercial no puede exceder los 255 caracteres.");

        RuleFor(x => x.CodigoRegimenTributario)
            .NotEmpty().WithMessage("El régimen tributario es obligatorio.");

        RuleFor(x => x.DireccionFiscal)
            .NotEmpty().WithMessage("La dirección fiscal es obligatoria.");

        RuleFor(x => x.Ubigeo)
            .NotEmpty().WithMessage("El ubigeo es obligatorio.")
            .Length(6).WithMessage("El ubigeo debe tener exactamente 6 caracteres.")
            .Matches("^[0-9]+$").WithMessage("El ubigeo debe contener solo dígitos.");

        RuleFor(x => x.MonedaBase)
            .NotEmpty().WithMessage("La moneda base es obligatoria.")
            .Length(3).WithMessage("La moneda base debe tener exactamente 3 caracteres.");
    }
}