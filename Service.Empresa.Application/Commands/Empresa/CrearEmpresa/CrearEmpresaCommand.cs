using MediatR;
using Service.Empresa.Application.DTOs.Empresa;

namespace Service.Empresa.Application.Commands.Empresa.CrearEmpresa;

public class CrearEmpresaCommand : IRequest<CrearEmpresaDTO>
{
    public string Ruc { get; set; } = "";
    public string RazonSocial { get; set; } = "";
    public string NombreComercial { get; set; } = "";
    public string CodigoRegimenTributario { get; set; } = "";
    public string CodigoEstadoContribuyente { get; set; } = "";
    public string CodigoCondicionContribuyente { get; set; } = "";
    public string DireccionFiscal { get; set; } = "";
    public string Ubigeo { get; set; } = "";
    public string MonedaBase { get; set; } = "PEN";
    public string? LogoUrl { get; set; }
    public string CreadoPor { get; set; } = "";
    public Guid IdUsuario { get; set; }
}