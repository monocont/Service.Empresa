using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Commands.CredencialSunat.Crear;

public class CrearCredencialSunatCommandHandler : IRequestHandler<CrearCredencialSunatCommand, CredencialSunatDTO>
{
    private readonly ICredencialSunatRepository _credencialRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;
    private readonly IEmpresaRepository _empresaRepository;

    public CrearCredencialSunatCommandHandler(
        ICredencialSunatRepository credencialRepository,
        IUsuarioEmpresaRepository usuarioEmpresaRepository,
        IEmpresaRepository empresaRepository)
    {
        _credencialRepository = credencialRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
        _empresaRepository = empresaRepository;
    }

    public async Task<CredencialSunatDTO> Handle(CrearCredencialSunatCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.IdEmpresa);
        if (empresa is null)
            throw new ArgumentException("La empresa no existe.");
        if (!request.EsAdmin)
        {
            var acceso = await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(request.IdUsuario, empresa.IdEmpresa);
            if (acceso is null)
                throw new UnauthorizedAccessException("No tiene acceso a esta empresa.");
        }


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