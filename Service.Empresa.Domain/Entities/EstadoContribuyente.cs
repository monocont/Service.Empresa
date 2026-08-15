using Service.Empresa.Domain.Entities;

namespace Service.Empresa.Domain.Entities;

public class EstadoContribuyente : EntidadAuditoria
{
    public string Codigo { get; protected set; } = "";
    public string Descripcion { get; protected set; } = "";
    protected EstadoContribuyente() { }

    public string ObtenerDescripcion() => Descripcion;

    public static EstadoContribuyente New(string codigo, string descripcion)
    {
        return new EstadoContribuyente
        {
            Codigo = codigo,
            Descripcion = descripcion,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };
    }
}