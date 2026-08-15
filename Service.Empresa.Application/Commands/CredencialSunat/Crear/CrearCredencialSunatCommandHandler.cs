using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.CredencialSunat.Crear;

public class CrearCredencialSunatCommandHandler : IRequestHandler<CrearCredencialSunatCommand, CredencialSunatDTO>
{
    private readonly ICredencialSunatRepository _credencialRepository;
    private readonly IEmpresaRepository _empresaRepository;

    public CrearCredencialSunatCommandHandler(
        ICredencialSunatRepository credencialRepository,
        IEmpresaRepository empresaRepository)
    {
        _credencialRepository = credencialRepository;
        _empresaRepository = empresaRepository;
    }

    public async Task<CredencialSunatDTO> Handle(CrearCredencialSunatCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.IdEmpresa);
        if (empresa is null)
            throw new ArgumentException("La empresa no existe.");

        var existe = await _credencialRepository.ObtenerPorEmpresaAsync(request.IdEmpresa);
        if (existe is not null)
            throw new ArgumentException("La empresa ya tiene una credencial SUNAT registrada.");

        var credencial = Domain.Entities.CredencialSunat.Crear(
            request.IdEmpresa,
            request.UsuarioSol.ToUpper(),
            request.ClaveSol,
            request.CreadoPor);

        await _credencialRepository.AgregarAsync(credencial);
        await _credencialRepository.CommitAsync();

        return new CredencialSunatDTO
        {
            IdCredencial = credencial.IdCredencial.ToString(),
            IdEmpresa = credencial.IdEmpresa.ToString(),
            UsuarioSol = credencial.UsuarioSol
        };
    }
}