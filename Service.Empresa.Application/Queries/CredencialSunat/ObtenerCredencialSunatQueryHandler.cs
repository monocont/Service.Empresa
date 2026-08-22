using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.CredencialSunat;

public class ObtenerCredencialSunatQueryHandler : IRequestHandler<ObtenerCredencialSunatQuery, CredencialSunatDTO?>
{
    private readonly ICredencialSunatRepository _credencialRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public ObtenerCredencialSunatQueryHandler(ICredencialSunatRepository credencialRepository, IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _credencialRepository = credencialRepository;
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<CredencialSunatDTO?> Handle(ObtenerCredencialSunatQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.IdEmpresa);
        if (empresa is null)
            return null;


        if (!request.EsAdmin)
        {
            var acceso = await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(request.IdUsuario, empresa.IdEmpresa);
            if (acceso is null)
                throw new UnauthorizedAccessException("No tiene acceso a esta empresa.");
        }

        var credencial = await _credencialRepository.ObtenerPorEmpresaAsync(request.IdEmpresa);
        if (credencial is null)
            return null;

        return new CredencialSunatDTO
        {
            IdCredencial = credencial.IdCredencial.ToString(),
            IdEmpresa = credencial.IdEmpresa.ToString(),
            UsuarioSol = credencial.UsuarioSol,
            ClaveSol = credencial.ClaveSol
        };
    }
}