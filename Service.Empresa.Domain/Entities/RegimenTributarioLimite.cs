namespace Service.Empresa.Domain.Entities;

public class RegimenTributarioLimite : EntidadAuditoria
{
    public int IdLimite { get; private set; }
    public string CodigoRegimenTributario { get; private set; } = string.Empty;
    public int Anio { get; private set; }
    public decimal ValorUit { get; private set; }
    public decimal? LimiteMensualVentas { get; private set; }
    public decimal? LimiteMensualCompras { get; private set; }
    public decimal? LimiteAnualVentas { get; private set; }
    public decimal? LimiteAnualCompras { get; private set; }
    public int? LimiteAnualVentasUit { get; private set; }

    // Navigation property
    public virtual RegimenTributario? RegimenTributario { get; private set; }

    private RegimenTributarioLimite() { }

    public static RegimenTributarioLimite Crear(
        string codigoRegimenTributario,
        int anio,
        decimal valorUit,
        decimal? limiteMensualVentas,
        decimal? limiteMensualCompras,
        decimal? limiteAnualVentas,
        decimal? limiteAnualCompras,
        int? limiteAnualVentasUit,
        string usuarioCreacion)
    {
        return new RegimenTributarioLimite
        {
            CodigoRegimenTributario = codigoRegimenTributario,
            Anio = anio,
            ValorUit = valorUit,
            LimiteMensualVentas = limiteMensualVentas,
            LimiteMensualCompras = limiteMensualCompras,
            LimiteAnualVentas = limiteAnualVentas,
            LimiteAnualCompras = limiteAnualCompras,
            LimiteAnualVentasUit = limiteAnualVentasUit,
            Activo = true,
            FechaCreacion = DateTime.UtcNow,
            CreadoPor = usuarioCreacion
        };
    }

    public void Actualizar(
        decimal valorUit,
        decimal? limiteMensualVentas,
        decimal? limiteMensualCompras,
        decimal? limiteAnualVentas,
        decimal? limiteAnualCompras,
        int? limiteAnualVentasUit,
        string usuarioModificacion)
    {
        ValorUit = valorUit;
        LimiteMensualVentas = limiteMensualVentas;
        LimiteMensualCompras = limiteMensualCompras;
        LimiteAnualVentas = limiteAnualVentas;
        LimiteAnualCompras = limiteAnualCompras;
        LimiteAnualVentasUit = limiteAnualVentasUit;
        FechaModificacion = DateTime.UtcNow;
        ModificadoPor = usuarioModificacion;
    }
}
