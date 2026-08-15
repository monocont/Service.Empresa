using MediatR;
using Service.Empresa.Application.Common.Exceptions;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorRuc;

public class ObtenerEmpresaPorRucQueryHandler : IRequestHandler<ObtenerEmpresaPorRucQuery, ObtenerEmpresaPorRucDTO>
{
    private readonly IEmpresaRepository _empresaRepository;

    public ObtenerEmpresaPorRucQueryHandler(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<ObtenerEmpresaPorRucDTO> Handle(ObtenerEmpresaPorRucQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorRucAsync(request.Ruc);

        if (empresa is null)
            throw new NotFoundException($"No se encontró una empresa con el RUC {request.Ruc}.");

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