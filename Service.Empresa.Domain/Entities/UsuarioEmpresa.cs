namespace Service.Empresa.Domain.Entities;

/// <summary>
/// Relación de propiedad/acceso entre un usuario (GUID de Service.Seguridad) y una empresa.
/// Rol: DUENO | COLABORADOR.
/// </summary>
public class UsuarioEmpresa : EntidadAuditoria
{
    public Guid IdUsuarioEmpresa { get; private set; }
    public Guid IdUsuario { get; private set; }
    public Guid IdEmpresa { get; private set; }
    public string Rol { get; private set; } = "DUENO";

    private UsuarioEmpresa() { }

    public static UsuarioEmpresa Crear(Guid idUsuario, Guid idEmpresa, string rol, string? creadoPor)
    {
        return new UsuarioEmpresa
        {
            IdUsuarioEmpresa = Guid.NewGuid(),
            IdUsuario = idUsuario,
            IdEmpresa = idEmpresa,
            Rol = rol,
            CreadoPor = creadoPor,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };
    }
}
