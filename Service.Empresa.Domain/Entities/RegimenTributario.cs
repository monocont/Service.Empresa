using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Domain.Entities;

public class RegimenTributario : EntidadAuditoria
{
    public string Codigo { get; protected set; } = "";
    public string Descripcion { get; protected set; } = "";
    protected RegimenTributario() { }

    public string ObtenerDescripcion() => Descripcion;

    public static RegimenTributario New(string codigo, string descripcion)
    {
        return new RegimenTributario
        {
            Codigo = codigo,
            Descripcion = descripcion,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };
    }
}