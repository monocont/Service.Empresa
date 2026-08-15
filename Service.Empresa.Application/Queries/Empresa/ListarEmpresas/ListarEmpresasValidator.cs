using FluentValidation;

namespace Service.Empresa.Application.Queries.Empresa.ListarEmpresas;

public class ListarEmpresasValidator : AbstractValidator<ListarEmpresasQuery>
{
    public ListarEmpresasValidator()
    {
        RuleFor(x => x.CreadoPor)
            .NotEmpty().WithMessage("El usuario 'CreadoPor' es obligatorio.");

        RuleFor(x => x.Ruc)
            .MaximumLength(11).WithMessage("El RUC no puede exceder los 11 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Ruc));

        RuleFor(x => x.RazonSocial)
            .MaximumLength(255).WithMessage("La razón social no puede exceder los 255 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.RazonSocial));

        RuleFor(x => x.CodigoRegimenTributario)
            .MaximumLength(10).WithMessage("El código de régimen tributario no puede exceder los 10 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.CodigoRegimenTributario));

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("El número de página debe ser mayor o igual a 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("El tamaño de página debe ser mayor o igual a 1.")
            .LessThanOrEqualTo(100).WithMessage("El tamaño de página no puede exceder los 100 registros.");
    }
}