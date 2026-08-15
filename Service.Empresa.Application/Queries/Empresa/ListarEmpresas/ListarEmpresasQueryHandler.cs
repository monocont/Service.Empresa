using MediatR;
using Service.Empresa.Application.DTOs.Empresa;
using Service.Empresa.Application.Interfaces;

namespace Service.Empresa.Application.Queries.Empresa.ListarEmpresas;

public class ListarEmpresasQueryHandler : IRequestHandler<ListarEmpresasQuery, ListarEmpresasDTO>
{
    private readonly IEmpresaRepository _empresaRepository;

    public ListarEmpresasQueryHandler(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
    }

    public async Task<ListarEmpresasDTO> Handle(ListarEmpresasQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _empresaRepository.ObtenerConFiltrosYPaginacionAsync(
            request.Ruc, 
            request.RazonSocial, 
            request.CodigoRegimenTributario,
            request.CreadoPor, 
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);

        var itemsDto = items.Select(e => new EmpresaItemDTO
        {
            IdEmpresa = e.IdEmpresa.ToString(),
            Ruc = e.Ruc,
            RazonSocial = e.RazonSocial,
            NombreComercial = e.NombreComercial,
            CodigoRegimenTributario = e.CodigoRegimenTributario,
            CodigoEstadoContribuyente = e.CodigoEstadoContribuyente,
            CodigoCondicionContribuyente = e.CodigoCondicionContribuyente
        }).ToList();

        var totalPages = (int)Math.Ceiling(total / (double)request.PageSize);

        return new ListarEmpresasDTO
        {
            Items = itemsDto,
            Total = total,
            TotalPages = totalPages,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}