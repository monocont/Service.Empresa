using MediatR;
using Service.Empresa.Application.Common.Exceptions;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorRuc;

public class ObtenerEmpresaPorRucQueryHandler : IRequestHandler<ObtenerEmpresaPorRucQuery, ObtenerEmpresaPorRucDTO>
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public ObtenerEmpresaPorRucQueryHandler(IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<ObtenerEmpresaPorRucDTO> Handle(ObtenerEmpresaPorRucQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorRucAsync(request.Ruc);

        if (empresa is null)
            throw new NotFoundException($"No se encontró una empresa con el RUC {request.Ruc}.");

        if (!request.EsAdmin)
        {
            var acceso = await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(request.IdUsuario, empresa.IdEmpresa);
            if (acceso is null)
                throw new UnauthorizedAccessException("No tiene acceso a esta empresa.");
        }

        return new ObtenerEmpresaPorRucDTO
        {
            IdEmpresa = empresa.IdEmpresa.ToString(),
            Ruc = empresa.Ruc,
            RazonSocial = empresa.RazonSocial,
            NombreComercial = empresa.NombreComercial,
            DireccionFiscal = empresa.DireccionFiscal,
            Ubigeo = empresa.Ubigeo,
            MonedaBase = empresa.MonedaBase,
            LogoUrl = empresa.LogoUrl,
            CodigoRegimenTributario = empresa.CodigoRegimenTributario,
            CodigoEstadoContribuyente = empresa.CodigoEstadoContribuyente,
            CodigoCondicionContribuyente = empresa.CodigoCondicionContribuyente
        };
    }
}