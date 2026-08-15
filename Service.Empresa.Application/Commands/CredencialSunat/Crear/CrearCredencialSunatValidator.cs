using FluentValidation;

namespace Service.Empresa.Application.Commands.CredencialSunat.Crear;

public class CrearCredencialSunatValidator : AbstractValidator<CrearCredencialSunatCommand>
{
    public CrearCredencialSunatValidator()
    {
        RuleFor(x => x.UsuarioSol)
            .NotEmpty().WithMessage("El usuario SOL es obligatorio.");

        RuleFor(x => x.ClaveSol)
            .NotEmpty().WithMessage("La clave SOL es obligatoria.");
    }
}