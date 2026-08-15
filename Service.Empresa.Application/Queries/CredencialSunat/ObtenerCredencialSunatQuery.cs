using MediatR;
using Service.Empresa.Application.DTOs.CredencialSunat;

namespace Service.Empresa.Application.Queries.CredencialSunat;

public class ObtenerCredencialSunatQuery : IRequest<CredencialSunatDTO?>
{
    public Guid IdEmpresa { get; set; }
}