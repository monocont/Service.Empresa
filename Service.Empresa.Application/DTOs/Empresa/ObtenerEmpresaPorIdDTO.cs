namespace Service.Empresa.Application.DTOs.Empresa;

public class ObtenerEmpresaPorIdDTO
{
    public string IdEmpresa { get; set; } = "";
    public string Ruc { get; set; } = "";
    public string RazonSocial { get; set; } = "";
    public string NombreComercial { get; set; } = "";
    public string DireccionFiscal { get; set; } = "";
    public string Ubigeo { get; set; } = "";
    public string MonedaBase { get; set; } = "";
    public string? LogoUrl { get; set; }
    public string CodigoRegimenTributario { get; set; } = "";
    public string CodigoEstadoContribuyente { get; set; } = "";
    public string CodigoCondicionContribuyente { get; set; } = "";
}