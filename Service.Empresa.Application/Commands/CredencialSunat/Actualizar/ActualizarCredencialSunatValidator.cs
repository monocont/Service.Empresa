using FluentValidation;

namespace Service.Empresa.Application.Commands.CredencialSunat.Actualizar;

public class ActualizarCredencialSunatValidator : AbstractValidator<ActualizarCredencialSunatCommand>
{
    public ActualizarCredencialSunatValidator()
    {
        RuleFor(x => x.UsuarioSol)
            .NotEmpty().WithMessage("El usuario SOL es obligatorio.");

        RuleFor(x => x.ClaveSol)
            .NotEmpty().WithMessage("La clave SOL es obligatoria.");
    }
}