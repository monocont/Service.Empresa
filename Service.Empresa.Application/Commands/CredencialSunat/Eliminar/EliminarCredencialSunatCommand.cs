using MediatR;

namespace Service.Empresa.Application.Commands.CredencialSunat.Eliminar;

public class EliminarCredencialSunatCommand : IRequest
{
    public Guid IdEmpresa { get; set; }
    public string IdCredencial { get; set; } = "";
    public Guid IdUsuario { get; set; }
    public bool EsAdmin { get; set; }
}
