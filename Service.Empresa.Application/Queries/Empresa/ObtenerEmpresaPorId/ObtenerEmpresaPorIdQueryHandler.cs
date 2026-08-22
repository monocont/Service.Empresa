using MediatR;
using Service.Empresa.Application.Common.Exceptions;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorId;

public class ObtenerEmpresaPorIdQueryHandler : IRequestHandler<ObtenerEmpresaPorIdQuery, ObtenerEmpresaPorIdDTO>
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IUsuarioEmpresaRepository _usuarioEmpresaRepository;

    public ObtenerEmpresaPorIdQueryHandler(IEmpresaRepository empresaRepository, IUsuarioEmpresaRepository usuarioEmpresaRepository)
    {
        _empresaRepository = empresaRepository;
        _usuarioEmpresaRepository = usuarioEmpresaRepository;
    }

    public async Task<ObtenerEmpresaPorIdDTO> Handle(ObtenerEmpresaPorIdQuery request, CancellationToken cancellationToken)
    {
        var empresa = await _empresaRepository.ObtenerPorIdAsync(request.Id);

        if (empresa is null)
            throw new NotFoundException(nameof(Domain.Entities.Empresa), request.Id);

        await ValidarAccesoAsync(request.IdUsuario, empresa.IdEmpresa, request.EsAdmin);

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

    private async Task ValidarAccesoAsync(Guid idUsuario, Guid idEmpresa, bool esAdmin)
    {
        if (esAdmin) return;

        var acceso = await _usuarioEmpresaRepository.ObtenerPorUsuarioYEmpresaAsync(idUsuario, idEmpresa);
        if (acceso is null)
            throw new UnauthorizedAccessException("No tiene acceso a esta empresa.");
    }
}
