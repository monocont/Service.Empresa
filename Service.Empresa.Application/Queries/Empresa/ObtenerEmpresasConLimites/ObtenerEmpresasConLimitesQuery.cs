using MediatR;
using Service.Empresa.Application.DTOs.Empresa;

namespace Service.Empresa.Application.Queries.Empresa.ObtenerEmpresasConLimites;

public class ObtenerEmpresasConLimitesQuery : IRequest<List<EmpresaConLimitesDTO>>
{
    public int Anio { get; set; }
    public Guid IdUsuario { get; set; }
    public bool EsAdmin { get; set; }
}
