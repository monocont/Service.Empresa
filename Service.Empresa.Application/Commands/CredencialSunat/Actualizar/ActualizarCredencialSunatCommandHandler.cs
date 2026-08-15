using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.CredencialSunat.Actualizar;

public class ActualizarCredencialSunatCommandHandler : IRequestHandler<ActualizarCredencialSunatCommand, CredencialSunatDTO>
{
    private readonly ICredencialSunatRepository _credencialRepository;

    public ActualizarCredencialSunatCommandHandler(ICredencialSunatRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task<CredencialSunatDTO> Handle(ActualizarCredencialSunatCommand request, CancellationToken cancellationToken)
    {
        var credencial = await _credencialRepository.ObtenerPorIdAsync(Guid.Parse(request.IdCredencial));
        if (credencial is null || credencial.IdEmpresa.ToString() != request.IdEmpresa.ToString())
            throw new ArgumentException("La credencial SUNAT no existe o no pertenece a la empresa.");

        credencial.Actualizar(
            request.UsuarioSol.ToUpper(),
            request.ClaveSol,
            request.CreadoPor);

        await _credencialRepository.CommitAsync();

        return new CredencialSunatDTO
        {
            IdCredencial = credencial.IdCredencial.ToString(),
            IdEmpresa = credencial.IdEmpresa.ToString(),
            UsuarioSol = credencial.UsuarioSol,
            ClaveSol = credencial.ClaveSol
        };
    }
}