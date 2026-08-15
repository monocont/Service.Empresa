using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.CredencialSunat;

public class ObtenerCredencialSunatQueryHandler : IRequestHandler<ObtenerCredencialSunatQuery, CredencialSunatDTO?>
{
    private readonly ICredencialSunatRepository _credencialRepository;

    public ObtenerCredencialSunatQueryHandler(ICredencialSunatRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task<CredencialSunatDTO?> Handle(ObtenerCredencialSunatQuery request, CancellationToken cancellationToken)
    {
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