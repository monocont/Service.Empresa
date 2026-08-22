using MediatR;
using Service.Empresa.Application.DTOs.Empresa;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresaPorId;

public class ObtenerEmpresaPorIdQuery : IRequest<ObtenerEmpresaPorIdDTO>
{
    public Guid Id { get; set; }
    public Guid IdUsuario { get; set; }
    public bool EsAdmin { get; set; }
}