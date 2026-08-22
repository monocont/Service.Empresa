using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;

namespace Service.Empresa.Application.Commands.CredencialSunat.Crear;

public class CrearCredencialSunatCommand : IRequest<CredencialSunatDTO>
{
    public Guid IdEmpresa { get; set; }
    public string UsuarioSol { get; set; } = "";
    public string ClaveSol { get; set; } = "";
    public string CreadoPor { get; set; } = "";
    public Guid IdUsuario { get; set; }
    public bool EsAdmin { get; set; }
}
