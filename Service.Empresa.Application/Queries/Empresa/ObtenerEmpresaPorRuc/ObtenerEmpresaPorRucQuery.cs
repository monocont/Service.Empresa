using MediatR;
using Service.Empresa.Application.DTOs.Empresa;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorRuc;

public class ObtenerEmpresaPorRucQuery : IRequest<ObtenerEmpresaPorRucDTO>
{
    public string Ruc { get; set; } = "";
}