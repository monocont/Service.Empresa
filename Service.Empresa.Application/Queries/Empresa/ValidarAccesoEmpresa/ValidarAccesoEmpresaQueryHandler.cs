using MediatR;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ValidarAccesoEmpresa;

/// <summary>
/// Determina si el usuario tiene acceso a la empresa del RUC indicado.
/// Es el endpoint de autorización que consumen los demás microservicios (delegación).
/// </summary>
public class ValidarAccesoEmpresaQueryHandler : IRequestHandler<ValidarAccesoEmpresaQuery, AccesoEmpresaDTO>
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public ValidarAccesoEmpresaQueryHandler(
        IEmpresaRepository empresaRepository,
        IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<AccesoEmpresaDTO> Handle(ValidarAccesoEmpresaQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorRucAsync(request.Ruc);
        if (empresa is null)
            return new AccesoEmpresaDTO { TieneAcceso = false, Rol = null };

        if (request.EsAdmin)
            return new AccesoEmpresaDTO { TieneAcceso = true, Rol = "ADMIN" };

        var acceso = await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(request.IdUsuario, empresa.IdEmpresa);
        return new AccesoEmpresaDTO { TieneAcceso = acceso is not null, Rol = acceso?.Rol };
    }
}
