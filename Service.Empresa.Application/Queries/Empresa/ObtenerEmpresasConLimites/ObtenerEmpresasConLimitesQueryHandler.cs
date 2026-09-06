using MediatR;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresasConLimites;

public class ObtenerEmpresasConLimitesQueryHandler : IRequestHandler<ObtenerEmpresasConLimitesQuery, List<EmpresaConLimitesDTO>>
{
    private readonly IEmpresaRepository _empresaRepository;

    public ObtenerEmpresasConLimitesQueryHandler(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<List<EmpresaConLimitesDTO>> Handle(ObtenerEmpresasConLimitesQuery request, CancellationToken cancellationToken)
    {
        return await _empresaRepository.ObtenerEmpresasConLimitesPorUsuarioAsync(
            request.IdUsuario,
            request.Anio,
            request.EsAdmin,
            cancellationToken);
    }
}
