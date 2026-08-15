using MediatR;
using Service.Empresa.Application.Common.Exceptions;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorId;

public class ObtenerEmpresaPorIdQueryHandler : IRequestHandler<ObtenerEmpresaPorIdQuery, ObtenerEmpresaPorIdDTO>
{
    private readonly IEmpresaRepository _empresaRepository;

    public ObtenerEmpresaPorIdQueryHandler(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<ObtenerEmpresaPorIdDTO> Handle(ObtenerEmpresaPorIdQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.Id);

        if (empresa is null)
            throw new NotFoundException(nameof(Domain.Entities.Empresa), request.Id);

        return new ObtenerEmpresaPorIdDTO
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