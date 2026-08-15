using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Domain.Entities;

public class CondicionContribuyente : EntidadAuditoria
{
    public string Codigo { get; protected set; } = "";
    public string Descripcion { get; protected set; } = "";
    protected CondicionContribuyente() { }

    public string ObtenerDescripcion() => Descripcion;

    public static CondicionContribuyente New(string codigo, string descripcion)
    {
        return new CondicionContribuyente
        {
            Codigo = codigo,
            Descripcion = descripcion,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };
    }
}