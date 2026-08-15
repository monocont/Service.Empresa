namespace Service.Empresa.Application.DTOs.Empresa;

public class EmpresaItemDTO
{
    public string IdEmpresa { get; set; } = "";
    public string Ruc { get; set; } = "";
    public string RazonSocial { get; set; } = "";
    public string NombreComercial { get; set; } = "";
    public string CodigoRegimenTributario { get; set; } = "";
    public string CodigoEstadoContribuyente { get; set; } = "";
    public string CodigoCondicionContribuyente { get; set; } = "";
}

public class ListarEmpresasDTO
{
    public List<EmpresaItemDTO> Items { get; set; } = [];
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}