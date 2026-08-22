using MediatR;

namespace Service.Empresa.Application.Queries.Empresa.ValidarAccesoEmpresa;

public class ValidarAccesoEmpresaQuery : IRequest<AccesoEmpresaDTO>
{
    public string Ruc { get; set; } = "";
    public Guid IdUsuario { get; set; }
    public bool EsAdmin { get; set; }
}
