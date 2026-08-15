using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Domain.Entities;

public class Empresa : EntidadAuditoria
{
    public Guid IdEmpresa { get; private set; }
    public string Ruc { get; set; } = "";
    public string RazonSocial { get; set; } = "";
    public string NombreComercial { get; set; } = "";
    public string DireccionFiscal { get; set; } = "";
    public string Ubigeo { get; set; } = "";
    public string MonedaBase { get; set; } = "PEN";
    public string? LogoUrl { get; set; }

    public string CodigoRegimenTributario { get; set; } = "";
    public string CodigoEstadoContribuyente { get; set; } = "";
    public string CodigoCondicionContribuyente { get; set; } = "";

    private Empresa() { }

    public static Empresa Crear(
        string ruc,
        string razonSocial,
        string nombreComercial,
        string codigoRegimenTributario,
        string codigoEstadoContribuyente,
        string codigoCondicionContribuyente,
        string direccionFiscal,
        string ubigeo,
        string monedaBase,
        string logoUrl,
        string usuarioCreacion)
    {
        return new Empresa
        {
            IdEmpresa = Guid.NewGuid(),
            Ruc = ruc,
            RazonSocial = razonSocial,
            NombreComercial = nombreComercial,
            CodigoRegimenTributario = codigoRegimenTributario,
            CodigoEstadoContribuyente = codigoEstadoContribuyente,
            CodigoCondicionContribuyente = codigoCondicionContribuyente,
            DireccionFiscal = direccionFiscal,
            Ubigeo = ubigeo,
            MonedaBase = monedaBase,
            LogoUrl = logoUrl,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            CreadoPor = usuarioCreacion
        };
    }

    public void Actualizar(
        string razonSocial,
        string nombreComercial,
        string codigoRegimenTributario,
        string codigoEstadoContribuyente,
        string codigoCondicionContribuyente,
        string direccionFiscal,
        string ubigeo,
        string monedaBase,
        string logoUrl,
        string usuarioModificacion)
    {
        RazonSocial = razonSocial;
        NombreComercial = nombreComercial;
        CodigoRegimenTributario = codigoRegimenTributario;
        CodigoEstadoContribuyente = codigoEstadoContribuyente;
        CodigoCondicionContribuyente = codigoCondicionContribuyente;
        DireccionFiscal = direccionFiscal;
        Ubigeo = ubigeo;
        MonedaBase = monedaBase;
        LogoUrl = logoUrl;
        ModificadoPor = usuarioModificacion;
        FechaModificacion = DateTime.UtcNow;
    }
}