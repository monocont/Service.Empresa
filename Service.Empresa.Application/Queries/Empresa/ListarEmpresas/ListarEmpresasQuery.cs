using MediatR;
using Service.Empresa.Application.DTOs.Empresa;

namespace Service.Empresa.Application.Queries.Empresa.ListarEmpresas;

public class ListarEmpresasQuery : IRequest<ListarEmpresasDTO>
{
    public Guid IdUsuario { get; set; }
    public string? Ruc { get; set; }
    public string? RazonSocial { get; set; }
    public string? CodigoRegimenTributario { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}